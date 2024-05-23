using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.PubSub;

public class DocumentUpdateListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IDocumentRepository<EducationDocument> _repository;

    public DocumentUpdateListener(IBus bus, IDocumentRepository<EducationDocument> repository)
    {
        _bus = bus;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateDocumentTypeMessage>("update_documents_subscription_id",
            UpdateDocuments);
    }

    private async Task UpdateDocuments(UpdateDocumentTypeMessage message)
    {
        var docs = await _repository.Find(x => x.EducationDocumentTypeId == message.EducationTypeId);
        foreach (var doc in docs)
        {
            var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(new GetUserEmailRequest(doc.UserId));
            await _bus.PubSub.PublishAsync(new UpdatedToEmailMessage(email.Email, $"Your education document with name {doc.Name} was deleted. \\n" +
                $"Such education document type was removed from system"));

        }
    }
}