using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;

namespace UserService.Application.ServiceBus.PubSub.Sender;

public class PubSubSender
{
    private readonly IBus _bus;

    public PubSubSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task UpdateStatus(Guid userId)
    {
        await _bus.PubSub.PublishAsync(new UpdateStatusMessage(userId));
    }
    
}