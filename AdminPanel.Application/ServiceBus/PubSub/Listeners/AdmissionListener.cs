using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Domain.Entities;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdminPanel.Application.ServiceBus.PubSub.Listeners;

public class AdmissionListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;


    public AdmissionListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<AdmissionMessage>("admission_creation_subscription_id", CreateAdmission);
    }

    private async Task CreateAdmission(AdmissionMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            
            if (await repository.CheckExistence(message.AdmissionId))
            {
                var admission = await repository.GetById(message.AdmissionId);
                await repository.DeleteAsync(admission);
            }
            else
            {
                await repository.CreateAsync(new Admission
                {
                    Id = message.AdmissionId,
                    ManagerId = null
                });
            }
        }
    }
}