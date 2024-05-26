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
    
    public void UpdateApplicantRole(Guid userId)
    {
        _bus.PubSub.Publish(new UpdateUserRoleMessage(userId, "Applicant"));
    }

    public void Admission(Guid admissionId)
    {
        _bus.PubSub.Publish(new AdmissionMessage(admissionId));
    }
}