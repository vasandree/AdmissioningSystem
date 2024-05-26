using AutoMapper;
using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using Common.ServiceBus.RabbitMqMessages;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Features.Queries.GetByEmail;

namespace UserService.Application.ServiceBus.RPC.RPCHandler;

public class UserServiceRpcHandler : BaseRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public UserServiceRpcHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public override void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<AdminRoleCheckRequest, AdminRoleCheckResponse>(async (request) =>
            HandleException(await CheckAdmin(request)));

        _bus.Rpc.RespondAsync<GetUserEmailRequest, GetUserEmailResponse>(async (request) =>
            HandleException(await GetUserEmail(request)));

        _bus.Rpc.RespondAsync<GetUserRequest, GetUserResponse>(async (request) =>
            HandleException(await GetUser(request)));

        _bus.Rpc.RespondAsync<GetApplicantIdsRequest, GetApplicantIdsResponse>(async (request) =>
            HandleException(await GetUserIds(request)));

        _bus.Rpc.RespondAsync<GetUserInfoRequest, GetUserInfoResponse>(async (request) =>
            HandleException(await GetUserInfo(request)));

        _bus.Rpc.RespondAsync<GetAllApplicantsRequest, GetAllApplicantsResponse>(async (request) =>
            HandleException(await GetApplicants(request)));
        
        _bus.Rpc.RespondAsync<GetAllUsersRequest, GetAllUsersResponse>(async (request) =>
            HandleException(await GetUsers(request)));
    }

    private async Task<GetAllUsersResponse> GetUsers(GetAllUsersRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var applicants = await userManager.GetUsersInRoleAsync("Applicant");
            var userDtos = mapper.Map<IEnumerable<UserDto>>(applicants);


            return new GetAllUsersResponse(userDtos.ToList());
        }    
    }

    private async Task<GetAllApplicantsResponse> GetApplicants(GetAllApplicantsRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var applicants = await userManager.GetUsersInRoleAsync("Applicant");
            var applicantDtos = mapper.Map<IEnumerable<UserDto>>(applicants);


            return new GetAllApplicantsResponse(applicantDtos.ToList());
        }
    }

    private async Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            return new GetUserInfoResponse(await mediatr.Send(new GetUserByIdQuery(request.ApplicantId)));
        }
    }

    private async Task<GetApplicantIdsResponse> GetUserIds(GetApplicantIdsRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<UserManager<ApplicantEntity>>();

            var users = await userRepository.Users.AsQueryable().Where(x => x.User.FullName.Contains(request.Name))
                .Select(x => x.Id).ToListAsync();

            return new GetApplicantIdsResponse(users);
        }
    }

    private async Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await userRepository.GetById(request.UserId);

            if (user == null)
                return new GetUserResponse(new NotFound("User does not exist"));

            return new GetUserResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.PasswordHash
            };
        }
    }

    private async Task<GetUserEmailResponse> GetUserEmail(GetUserEmailRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetById(request.UserId);


            return new GetUserEmailResponse(user!.Email);
        }
    }

    private async Task<AdminRoleCheckResponse> CheckAdmin(AdminRoleCheckRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetById(request.UserId);
            if (user != null)
            {
                var roles = await userRepository.GetUserRoles(user);
                return new AdminRoleCheckResponse(roles.Contains(request.RequiredRole));
            }

            return new AdminRoleCheckResponse(false,
                new Forbidden("You don't have permission to perform this action."));
        }
    }
}