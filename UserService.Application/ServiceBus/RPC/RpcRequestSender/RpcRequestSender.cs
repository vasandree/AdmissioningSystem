using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;

namespace UserService.Application.ServiceBus.RPC.RpcRequestSender;

public class RpcRequestSender
{
    private readonly IBus _bus;

    public RpcRequestSender(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task<bool> CheckStatusClosedByUserId(Guid userId)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckStatusClosedRequest, CheckAdmissionStatusResponse>(
                new CheckStatusClosedRequest(userId));

        return response.Closed;
    }
}