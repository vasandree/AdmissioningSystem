using AdmissionService.Application.Contracts.Persistence;
using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.PubSubListener;

public class ProgramDeletionListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IAdmissionRepository _repository;

    public ProgramDeletionListener(IBus bus, IAdmissionRepository repository)
    {
        _bus = bus;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ProgramsToDeleteMessage>("programs_deletion_subscriptio _id",
            SoftDeleteAdmissions);
    }

    private async Task SoftDeleteAdmissions(ProgramsToDeleteMessage message)
    {
        var admissionsToDelete = await _repository.GetAdmissionsByProgramIds(message.ProgramsToDelete);
        foreach (var admission in admissionsToDelete)
        {
            var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(new GetUserEmailRequest(admission.ApplicantId));
            _bus.PubSub.Publish(new DeletedToEmailMessage(email.Email, $"Your admission ${admission.AdmissionId} for program {admission.ProgramId} was deleted." +
                                                                       $"Because chosen program was removed from system"));
        }
    }
}