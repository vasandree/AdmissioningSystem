using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.ServiceBus.RPC;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class DeleteManagerFromAdmissionListener: BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public DeleteManagerFromAdmissionListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateManagerFromAdmissionMessage>("manager_deletion_subscription_id",
            UpdateManager);
        //todo: check
    }

    private async Task UpdateManager(UpdateManagerFromAdmissionMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admission = await repository.GetById(message.AdmissionId);
            admission.ManagerId = message.ManagerId;
            
            if (admission.Status == AdmissionStatus.UnderConsideration)
            {
                admission.Status = AdmissionStatus.Created;

                var email = await rpc.GetEmail(admission.ApplicantId);
                _bus.PubSub.Publish(new UpdatedToEmailMessage(email,
                    $"Status of your admission {admission.AdmissionId} for program with id={admission.ProgramId} was changed to {AdmissionStatus.Created}"));
            }
            else if (admission.Status == AdmissionStatus.Created)
            {
                admission.Status = AdmissionStatus.UnderConsideration;

                var email = await rpc.GetEmail(admission.ApplicantId);
                _bus.PubSub.Publish(new UpdatedToEmailMessage(email,
                    $"Status of your admission {admission.AdmissionId} for program with id={admission.ProgramId} was changed to {AdmissionStatus.UnderConsideration}"));

            }
            

            await repository.UpdateAsync(admission);

        }
    }
}