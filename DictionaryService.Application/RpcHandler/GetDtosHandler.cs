using AutoMapper;
using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using Common.ServiceBus.RabbitMqMessages;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.RpcHandler;

public class GetDtosHandler : BaseRpcHandler
{
    private readonly IBus _bus;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public GetDtosHandler(IBus bus, IServiceProvider serviceProvider, IMapper mapper)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public override void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<EducationDocumentTypeCheckRequest, EducationDocumentTypeCheckResponse>(async (request) =>
            HandleException(await CheckDocumentType(request.DocumentTypeId)));

        _bus.Rpc.RespondAsync<GetDocumentTypeDtoRequest, GetDocumentTypeDtoResponse>(async (request) =>
            HandleException(await GetDocumentType(request.DocumentTypeId)));

        _bus.Rpc.RespondAsync<GetEducationDocumentTypeRequest, GetEducationDocumentResponse>(async (request) =>
            HandleException(await GetEducationDocument(request.DocumentTypeId)));

        _bus.Rpc.RespondAsync<GetProgramDtoRequest, GetProgramDtoResponse>(async (request) =>
            HandleException(await GetProgramDto(request.ProgramId)));

        _bus.Rpc.RespondAsync<CheckFacultyRequest, CheckFacultyResponse>(async (request) =>
            HandleException(await CheckFaculty(request)));

        _bus.Rpc.RespondAsync<GetProgramIdsRequest, GetProgramIdsResponse>(async (request) =>
            HandleException(await GetProgramIds(request)));
    }

    private async Task<GetProgramIdsResponse> GetProgramIds(GetProgramIdsRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IProgramRepository>();

            var programs = repository.GetAllAsQueryable();

            var res =  programs.Where(x => request.FacultyIds.Contains(x.Faculty.Id)).Include(x => x.Faculty)
                .Select(x => x.Id).ToList();

            return new GetProgramIdsResponse(res);
        }
    }

    private async Task<CheckFacultyResponse> CheckFaculty(CheckFacultyRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IFacultyRepository>();

            if (!await repository.CheckIfExists(request.FacultyId))
                return new CheckFacultyResponse(false, new NotFound("Provided document type does not exist"));

            if (!await repository.CheckIfNotDeleted(request.FacultyId))
                return new CheckFacultyResponse(false, new NotFound("Provided document wsa deleted"));

            return new CheckFacultyResponse(true);
        }
    }

    private async Task<EducationDocumentTypeCheckResponse> CheckDocumentType(Guid documentTypeId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();

            if (!await repository.CheckExistenceById(documentTypeId))
                return new EducationDocumentTypeCheckResponse(false,
                    new NotFound("Provided document type does not exist"));

            if (await repository.CheckIfNotDeleted(documentTypeId))
                return new EducationDocumentTypeCheckResponse(false, new NotFound("Provided document wsa deleted"));

            return new EducationDocumentTypeCheckResponse(true);
        }
    }


    private async Task<GetProgramDtoResponse> GetProgramDto(Guid programId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var programRepository = scope.ServiceProvider.GetRequiredService<IProgramRepository>();

            if (!await programRepository.CheckExistenceById(programId))
                return new GetProgramDtoResponse(null, new NotFound("Provided program does not exist"));

            var program = await programRepository.GetById(programId);

            if (program.IsDeleted)
                return new GetProgramDtoResponse(null, new NotFound("Provided program was deleted"));

            return new GetProgramDtoResponse(_mapper.Map<ProgramDto>(program));
        }
    }

    private async Task<GetDocumentTypeDtoResponse> GetDocumentType(Guid documentTypeId)
    {
        var (dto, exception) = await GetDocumentTypeDto(documentTypeId);
        return new GetDocumentTypeDtoResponse(dto, exception);
    }

    private async Task<GetEducationDocumentResponse> GetEducationDocument(Guid documentTypeId)
    {
        var (dto, exception) = await GetDocumentTypeDto(documentTypeId);
        return new GetEducationDocumentResponse(dto, exception);
    }

    private async Task<(EducationDocumentTypeDto?, Exception?)> GetDocumentTypeDto(Guid documentId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var educationDocRepository = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
            var nextEducationLevelsRepository =
                scope.ServiceProvider.GetRequiredService<INextEducationLevelRepository>();

            if (educationDocRepository == null || nextEducationLevelsRepository == null)
            {
                Console.WriteLine("Dependencies are not initialized properly.");
                return (null, new Exception("Dependencies are not initialized properly."));
            }

            if (!await educationDocRepository.CheckExistenceById(documentId))
                return (null, new BadRequest("Provided document type does not exist"));

            var documentType = await educationDocRepository.GetById(documentId);

            if (documentType.IsDeleted)
                return (null, new BadRequest("Provided document type was deleted"));

            var nextEducationLevels = await nextEducationLevelsRepository.GetEducationLevels(documentType.Id);

            var dto = _mapper.Map<EducationDocumentTypeDto>(documentType);
            dto.NextEducationLevels = nextEducationLevels.Select(x => _mapper.Map<EducationLevelDto>(x)).ToList();

            return (dto, null);
        }
    }
}