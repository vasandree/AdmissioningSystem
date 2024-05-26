using Common.ServiceBus.RabbitMqMessages.Publish;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class UpdateDocumentInfoListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;


    public UpdateDocumentInfoListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateEducationDocMessage>("update_doc_subscription_id", UpdateDocumentInfo);
    }

    private async Task UpdateDocumentInfo(UpdateEducationDocMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();

            var document = (EducationDocument)await repository.GetByUserId(message.UserId);

            document.Name = message.Name;
            await repository.UpdateAsync(document);
        }
    }
}