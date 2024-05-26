using Common.ServiceBus.RabbitMqMessages.Publish;
using DocumentService.Application.Features.Commands.Documents.UploadDocument;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class UploadNewFileListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;


    public UploadNewFileListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UploadNewFileMessage>("upload_file_subscription_id", UploadNewFile);
    }

    private async Task UploadNewFile(UploadNewFileMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new UploadDocumentRequest(message.DocumentType, message.File, message.UserId));
        }
    }
}