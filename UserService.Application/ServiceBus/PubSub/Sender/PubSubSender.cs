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

    public void UpdateStatus(Guid userId)
    {
        _bus.PubSub.Publish(new UpdateStatusMessage(userId));
    }
    
}