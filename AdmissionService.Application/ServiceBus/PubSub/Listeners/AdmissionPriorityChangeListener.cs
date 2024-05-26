using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Features.Commands.EditPriority;
using AdmissionService.Application.ServiceBus.RPC;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdmissionService.Application.ServiceBus.PubSub.Listeners;

public class AdmissionPriorityChangeListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public AdmissionPriorityChangeListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateAdmissionPriorityMessage>("priority_cahnge_subscription_id",
            UpdatePriority);
    }

    private async Task UpdatePriority(UpdateAdmissionPriorityMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAdmissionRepository>();
            var rpc = scope.ServiceProvider.GetRequiredService<RpcRequestsSender>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var admission = await repository.GetById(message.AdmissionId);
            await repository.DeleteAsync(admission);

            await mediator.Send(new EditPriorityCommand(admission.ApplicantId, admission.AdmissionId,
                message.NewPriority));

            var email = await rpc.GetEmail(admission.ApplicantId);

            _bus.PubSub.Publish(new UpdatedToEmailMessage(email,
                $"Priority of your admission {admission.AdmissionId} for program with id={admission.ProgramId} was changed"));
        }
    }
}