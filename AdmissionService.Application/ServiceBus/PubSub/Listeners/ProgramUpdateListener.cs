using AdmissionService.Application.Contracts.Persistence;
using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class ProgramUpdateListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ProgramUpdateListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ProgramToUpdateMessage>("program_update_subscription_id",
            UpdateProgram);
        //todo: check
    }

    private async Task UpdateProgram(ProgramToUpdateMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var admissionsToUpdate = await repository.GetAdmissionsByProgramId(message.ProgramId);
            foreach (var admission in admissionsToUpdate)
            {
                //todo: check
                var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(
                    new GetUserEmailRequest(admission.ApplicantId));
                _bus.PubSub.Publish(new DeletedToEmailMessage(email.Email,
                    $"Your admission ${admission.AdmissionId} for program {admission.ProgramId} was updated." +
                    $"Because chosen program was updated in system"));
            }
        }
        
    }
}