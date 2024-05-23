using AdmissionService.Application.Contracts.Persistence;
using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.PubSubListener;

public class ProgramUpdateListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IAdmissionRepository _repository;

    public ProgramUpdateListener(IBus bus, IAdmissionRepository repository)
    {
        _bus = bus;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ProgramToUpdateMessage>("program_update_subscription_id",
            UpdateProgram);
    }

    private async Task UpdateProgram(ProgramToUpdateMessage message)
    {
        var admissionsToUpdate = await _repository.GetAdmissionsByProgramId(message.ProgramId);
        foreach (var admission in admissionsToUpdate)
        {
            var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(
                new GetUserEmailRequest(admission.ApplicantId));
            _bus.PubSub.Publish(new DeletedToEmailMessage(email.Email,
                $"Your admission ${admission.AdmissionId} for program {admission.ProgramId} was updated." +
                $"Because chosen program was updated in system"));
        }
    }
}