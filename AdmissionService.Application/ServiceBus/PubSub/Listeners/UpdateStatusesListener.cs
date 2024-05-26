using AdmissionService.Application.Contracts.Persistence;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class UpdateStatusesListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public UpdateStatusesListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<UpdateStatusMessage>("update_statuses_subscription_id",
            UpdateStatuses);
    }

    private async Task UpdateStatuses(UpdateStatusMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();

            var admissions = await repository.GetApplicantsAdmissions(message.UserId);

            var admissionsToUpdate = admissions.Where(x =>
                x.Status == AdmissionStatus.Confirmed || x.Status == AdmissionStatus.Rejected);

            foreach (var admission in admissionsToUpdate)
            {
                if (admission.ManagerId != null)
                    admission.Status = AdmissionStatus.UnderConsideration;
                else
                    admission.Status = AdmissionStatus.Created;

                await repository.UpdateAsync(admission);
            }
        }
    }
}