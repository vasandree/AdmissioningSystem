using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Features.Queries.GetAllMyAdmissions;
using AdmissionService.Domain.Entities;
using AutoMapper;
using Common.Models.Exceptions;
using Common.Models.Models;
using Common.Models.Models.Dtos;
using Common.Models.Models.Dtos.PagedDtos;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AdmissionDto = Common.Models.Models.Dtos.AdmissionDto;

namespace AdmissionService.Application.ServiceBus.RPC;

public class RpcHandler : BaseRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;


    public RpcHandler(IBus bus, IServiceProvider serviceProvider, IMapper mapper)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public override async void CreateRequestListeners()
    {
        await _bus.Rpc.RespondAsync<CheckAdmissionStatusRequest, CheckAdmissionStatusResponse>(async (request) =>
            HandleException(await CheckAdmissionStatus(request)));

        await _bus.Rpc.RespondAsync<CheckPriorityAvailableRequest, CheckPriorityAvailableResponse>(async (request) =>
            HandleException(await CheckIfNewPriorityAvailable(request)));

        await _bus.Rpc.RespondAsync<CheckManagerFacultyRequest, CheckManagerFacultyResponse>(async (request) =>
            HandleException(await CheckFaculty(request)));

        await _bus.Rpc.RespondAsync<CheckNotMyAdmissionRequest, CheckNotMyResponse>(async (request) =>
            HandleException(await CheckNotMyAdmission(request)));

        await _bus.Rpc.RespondAsync<CheckFacultyByUserIdRequest, CheckFacultyByUserIdResponse>(async (request) =>
            HandleException(await CheckFacultyByUserId(request)));

        await _bus.Rpc.RespondAsync<CheckStatusClosedRequest, CheckAdmissionStatusResponse>(async (request) =>
            HandleException(await CheckIfAnyOfAdmissionsClosed(request)));

        await _bus.Rpc.RespondAsync<GetAllAdmissionsRequest, GetAllAdmissionsResponse>(async (request) =>
            HandleException(await GetAdmissions(request)));

        await _bus.Rpc.RespondAsync<CheckManagersApplicantRequest, CheckNotMyResponse>(async (request) =>
            HandleException(await CheckManagersApplicant(request)));
        
        
        await _bus.Rpc.RespondAsync<GetAllApplicantAdmissionsRequest, GetAllApplicantAdmissionsResponse>(async (request) =>
            HandleException( await GetApplicantsAdmissions(request)));

        
    }

    private async Task<GetAllApplicantAdmissionsResponse> GetApplicantsAdmissions(GetAllApplicantAdmissionsRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            return new GetAllApplicantAdmissionsResponse(await mediatr.Send(new GetAllMyAdmissionsCommand(request.ApplicantId)));
        }
        
        
    }


    private async Task<CheckNotMyResponse> CheckManagersApplicant(CheckManagersApplicantRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admissions = await repository.GetApplicantsAdmissions(request.ApplicantId);

            return new CheckNotMyResponse(admissions.Any(x => x.ManagerId == request.ManagerId) == false);
        }
    }

    private async Task<GetAllAdmissionsResponse> GetAdmissions(GetAllAdmissionsRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admissions = repository.GetAsQueryable();

            admissions = GetByProgram(admissions, request.Program);
            admissions = GetByManager(admissions, request.ManagerId);
            admissions = GetWithNoManagers(admissions, request.NoManager);
            admissions = GetByStatus(admissions, request.Status);
            admissions = await GetByFaculties(admissions, request.Faculties);
            admissions = await GetByName(admissions, request.Name);


            var total = await admissions.CountAsync();
            var totalPages = (int)Math.Ceiling((double)total / request.Size);

            if (totalPages < request.Page && request.Page > 1)
                return new GetAllAdmissionsResponse(null, new NotFound("Invalid value for attribute page"));

            if (totalPages < request.Page && request.Page == 1)
                return new GetAllAdmissionsResponse(null, new NotFound("No programs were found"));

            var result = admissions.Skip(request.Size * (request.Page - 1))
                .Take(request.Size)
                .ToList();

            List<AdmissionDto> list = new List<AdmissionDto>();
            foreach (var admission in result)
            {
                var dto = _mapper.Map<AdmissionDto>(admission);
                var applicant = await rpc.GetApplicant(admission.ApplicantId);
                var applicantDto = new ApplicantDto
                {
                    Id = applicant.UserId,
                    FullName = applicant.FullName,
                    Email = applicant.Email
                };
                dto.Applicant = applicantDto;
                list.Add(dto);
            }

            var resultDto = new AdmissionsPagedDto
            {
                Admissions = list,
                Pagination = new Pagination(request.Size, totalPages, request.Page)
            };

            return new GetAllAdmissionsResponse(resultDto);
        }
    }

    private async Task<IQueryable<Admission>> GetByName(IQueryable<Admission> admissions, string? name)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            if (string.IsNullOrEmpty(name)) return admissions;

            var applicantIds = await rpc.GetApplicants(name);

            return admissions.Where(x => applicantIds.Contains(x.ProgramId));
        }
    }

    private async Task<IQueryable<Admission>> GetByFaculties(IQueryable<Admission> admissions, Guid[]? faculties)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            if (faculties == null) return admissions;

            var programIds = await rpc.GetProgramIds(faculties);

            return admissions.Where(x => programIds.Contains(x.ProgramId));
        }
    }

    private IQueryable<Admission> GetByStatus(IQueryable<Admission> admissions, AdmissionStatus? status)
    {
        return status != null ? admissions.Where(x => x.Status == status) : admissions;
    }

    private IQueryable<Admission> GetWithNoManagers(IQueryable<Admission> admissions, bool noManager)
    {
        return noManager ? admissions.Where(x => x.ManagerId == null) : admissions;
    }

    private IQueryable<Admission> GetByManager(IQueryable<Admission> admissions, Guid? managerId)
    {
        return managerId != null ? admissions.Where(x => x.ManagerId == managerId) : admissions;
    }

    private IQueryable<Admission> GetByProgram(IQueryable<Admission> admissions, Guid? program)
    {
        return program != null ? admissions.Where(x => x.ProgramId == program) : admissions;
    }

    private async Task<CheckAdmissionStatusResponse> CheckIfAnyOfAdmissionsClosed(CheckStatusClosedRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admissions = await repository.GetApplicantsAdmissions(request.UserId);

            return new CheckAdmissionStatusResponse(admissions.Any(x => x.Status == AdmissionStatus.Closed));
        }
    }

    private async Task<CheckFacultyByUserIdResponse> CheckFacultyByUserId(CheckFacultyByUserIdRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var admissionRepository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admissions = await admissionRepository.GetApplicantsAdmissions(request.UserId);

            foreach (var admission in admissions)
            {
                var program = await rpc.GetProgram(admission.ProgramId);
                if (program != null)
                {
                    if (program.Faculty.Id == request.FacultyId)
                        return new CheckFacultyByUserIdResponse(true);
                }
            }

            return new CheckFacultyByUserIdResponse(false);
        }
    }

    private async Task<CheckNotMyResponse> CheckNotMyAdmission(CheckNotMyAdmissionRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var admissionRepository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admission = await admissionRepository.GetById(request.AdmissionId);

            return new CheckNotMyResponse(admission.ApplicantId != request.ManagerId);
        }
    }

    private async Task<CheckManagerFacultyResponse> CheckFaculty(CheckManagerFacultyRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var admissionRepository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admission = await admissionRepository.GetById(request.AdmissionId);
            var program = await rpc.GetProgram(admission.ProgramId);

            if (program == null)
                return new CheckManagerFacultyResponse(false, new NotFound("Provided faculty does not exist"));

            if (program.Faculty.Id == request.FacultyId) return new CheckManagerFacultyResponse(true);

            return new CheckManagerFacultyResponse(false);
        }
    }

    private async Task<CheckAdmissionStatusResponse> CheckAdmissionStatus(CheckAdmissionStatusRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var admissionRepository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admission = await admissionRepository.GetById(request.AdmissionId);

            if (admission.Status == AdmissionStatus.Closed)
                return new CheckAdmissionStatusResponse(true);

            var admissions = await admissionRepository.GetApplicantsAdmissions(admission.ApplicantId);

            if (admissions.Any(x => x.Status == AdmissionStatus.Closed))
                return new CheckAdmissionStatusResponse(true);

            return new CheckAdmissionStatusResponse(false);
        }
    }


    private async Task<CheckPriorityAvailableResponse> CheckIfNewPriorityAvailable(
        CheckPriorityAvailableRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admission = await repository.GetById(request.AdmissionId);

            var available = repository.CheckIfNewPriorityIsAvailable(admission.ApplicantId, request.NewPriority);

            return new CheckPriorityAvailableResponse(available);
        }
    }
}