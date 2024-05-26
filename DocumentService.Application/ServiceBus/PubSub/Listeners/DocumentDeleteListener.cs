using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class DocumentDeleteListener : BackgroundService
{

    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public DocumentDeleteListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         await _bus.PubSub.SubscribeAsync<DocumentsToDeleteMessage>("delete_documents_subscription_id",
            SoftDeleteDocuments);
    }

    private async Task SoftDeleteDocuments(DocumentsToDeleteMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();
            
            var docsToDelete = await repository.GetIdsToDelete(message.DocumentIdsToDelete);
            foreach (var doc in docsToDelete)
            {
                await repository.SoftDelete(doc);
                var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(new GetUserEmailRequest(doc.UserId));
                await _bus.PubSub.PublishAsync(new DeletedToEmailMessage(email.Email, $"Your education document with name {doc.Name} was deleted. \\n" +
                    $"Such education document type was removed from system"));
            }
        }
        
    }
}