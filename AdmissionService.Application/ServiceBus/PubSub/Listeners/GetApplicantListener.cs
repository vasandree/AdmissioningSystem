using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.ServiceBus.RPC;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class GetApplicantListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public GetApplicantListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<GetApplicantMessage>("applicant_update_subscription_id",
            UpdateStatus);
    }

    private async Task UpdateStatus(GetApplicantMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();

            var admission = await repository.GetById(message.AdmissionId);

            var user = await rpc.GetApplicant(admission.ApplicantId);
            
            _bus.PubSub.Publish(new EmailToApplicantMessage(user.Email, admission.AdmissionId));
        }
    }
}