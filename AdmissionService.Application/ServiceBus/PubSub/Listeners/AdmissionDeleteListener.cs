using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Features.Commands.DeleteAdmission;
using AdmissionService.Application.ServiceBus.RPC;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class AdmissionDeleteListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public AdmissionDeleteListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<DeleteAdmissionMessage>("admission_deletion_subscription_id",
            DeleteAdmission);
    }

    private async Task DeleteAdmission(DeleteAdmissionMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var admission = await repository.GetById(message.AdmissionId);
            await repository.DeleteAsync(admission);
            
            await mediator.Send(new DeleteAdmissionCommand(admission.ApplicantId, admission.AdmissionId));
            
            var email = await rpc.GetEmail(admission.ApplicantId);
            
            _bus.PubSub.Publish(new DeletedToEmailMessage(email, $"Your admission for program with id={admission.ProgramId} was deleted"));
        }
    }
}