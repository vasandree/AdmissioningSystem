using AdmissionService.Application.Contracts.Persistence;
using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class ProgramDeletionListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ProgramDeletionListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ProgramsToDeleteMessage>("programs_deletion_subscription_id",
            SoftDeleteAdmissions);
        //todo: check
    }

    private async Task SoftDeleteAdmissions(ProgramsToDeleteMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var admissionsToDelete = await repository.GetAdmissionsByProgramIds(message.ProgramsToDelete);
            foreach (var admission in admissionsToDelete)
            {
                //todo: check
                var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(
                    new GetUserEmailRequest(admission.ApplicantId));
                _bus.PubSub.Publish(new DeletedToEmailMessage(email.Email,
                    $"Your admission ${admission.AdmissionId} for program {admission.ProgramId} was deleted." +
                    $"Because chosen program was removed from system"));
            }
        }
    }
}