using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;

namespace AdmissionService.Application.ServiceBus.PubSub.Senders;

public class PubSubSender
{
    private readonly IBus _bus;

    public PubSubSender(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task UpdateApplicantRole(Guid userId)
    {
        await _bus.PubSub.PublishAsync(new UpdateUserRoleMessage(userId, "Applicant"));
    }

    public async Task Admission(Guid admissionId)
    {
        await _bus.PubSub.PublishAsync(new AdmissionMessage(admissionId));
    }
}