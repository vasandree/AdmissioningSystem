using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.RPCHandler;

public class GetUserInfoRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public GetUserInfoRpcHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }
    
    public void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<GetUserEmailRequest, GetUserEmailResponse>(async (request) =>
            await GetUserEmail(request));
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
}