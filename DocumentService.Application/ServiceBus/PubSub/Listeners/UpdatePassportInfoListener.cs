using Common.ServiceBus.RabbitMqMessages.Publish;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class UpdatePassportInfoListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;


    public UpdatePassportInfoListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<UpdatePassportMessage>("update_passport_info_subscription_id", UpdatePassport);
    }

    private async Task UpdatePassport(UpdatePassportMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IDocumentRepository<Passport>>();

            var document = (Passport)await repository.GetByUserId(message.UserId);

            document.SeriesAndNumber = message.SeriesAndNumber;
            document.IssueDate = message.IssueDate;
            document.IssuedBy = message.IssuedBy;
            document.DateOfBirth = message.DateOfBirth;
            await repository.UpdateAsync(document);
        }
    }
}