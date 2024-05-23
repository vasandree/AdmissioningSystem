using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.PubSub;

public class DocumentDeleteListener : BackgroundService
{

    private readonly IBus _bus;
    private readonly IDocumentRepository<EducationDocument> _repository;

    public DocumentDeleteListener(IBus bus, IDocumentRepository<EducationDocument> repository)
    {
        _bus = bus;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<DocumentsToDeleteMessage>("delete_documents_subscription_id",
            SoftDeleteDocuments);
    }

    private async Task SoftDeleteDocuments(DocumentsToDeleteMessage message)
    {
        var docsToDelete = await _repository.GetIdsToDelete(message.DocumentIdsToDelete);
        foreach (var doc in docsToDelete)
        {
            await _repository.SoftDelete(doc);
            var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(new GetUserEmailRequest(doc.UserId));
            await _bus.PubSub.PublishAsync(new DeletedToEmailMessage(email.Email, $"Your education document with name {doc.Name} was deleted. \\n" +
                                                                     $"Such education document type was removed from system"));
        }
    }
}