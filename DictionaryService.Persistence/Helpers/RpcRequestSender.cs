using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;

namespace DictionaryService.Persistence.Helpers;

public class RpcRequestSender
{
    private readonly IBus _bus;

    public RpcRequestSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task<AdminRoleCheckResponse> CheckIfAdmin(Guid userId)
    {
        return await _bus.Rpc.RequestAsync<AdminRoleCheckRequest, AdminRoleCheckResponse>(
            new AdminRoleCheckRequest(userId, "Admin"));

    }
}