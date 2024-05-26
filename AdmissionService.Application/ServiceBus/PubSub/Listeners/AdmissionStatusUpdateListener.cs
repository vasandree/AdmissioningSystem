using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.ServiceBus.RPC;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class AdmissionStatusUpdateListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public AdmissionStatusUpdateListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateAdmissionStatusMessage>("status_update_subscription_id",
            UpdateStatus);
    }

    private async Task UpdateStatus(UpdateAdmissionStatusMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admission = await repository.GetById(message.AdmissionId);


            if (admission.Status != AdmissionStatus.Closed)
            {
                admission.Status = message.NewStatus;
                await repository.UpdateAsync(admission);

                var email = await rpc.GetEmail(admission.ApplicantId);

                _bus.PubSub.Publish(new UpdatedToEmailMessage(email,
                    $"Status of your admission {admission.AdmissionId} for program with id={admission.ProgramId} was changed to {message.NewStatus}"));
            }
        }
    }
}