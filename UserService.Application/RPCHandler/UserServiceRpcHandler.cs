using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.RPCHandler;

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
            return new AdminRoleCheckResponse(false, new Forbidden("You don't have permission to perform this action."));
        }
    }
}