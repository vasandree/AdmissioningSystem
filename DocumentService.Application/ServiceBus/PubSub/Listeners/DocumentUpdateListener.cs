using Common.ServiceBus.RabbitMqMessages.Publish;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class DocumentUpdateListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    public DocumentUpdateListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         _bus.PubSub.Subscribe<UpdateDocumentTypeMessage>("update_documents_subscription_id",
            UpdateDocuments);
         //todo: check
    }

    private async Task UpdateDocuments(UpdateDocumentTypeMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();
            var docs = await repository.Find(x => x.EducationDocumentTypeId == message.EducationTypeId);
            foreach (var doc in docs)
            {
                var email = await _bus.Rpc.RequestAsync<GetUserEmailRequest, GetUserEmailResponse>(new GetUserEmailRequest(doc.UserId));
                //todo: check
                await _bus.PubSub.PublishAsync(new UpdatedToEmailMessage(email.Email, $"Your education document with name {doc.Name} was deleted. \\n" +
                    $"Such education document type was removed from system"));

            }
        }
        
    }
}