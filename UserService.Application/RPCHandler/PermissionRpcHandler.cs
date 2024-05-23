using EasyNetQ;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.RPCHandler;

public class PermissionRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public PermissionRpcHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<AdminRoleCheckRequest, AdminRoleCheckResponse>(async (request) =>
            await CheckAdmin(request));
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
            return new AdminRoleCheckResponse(false);
        }
    }
}
