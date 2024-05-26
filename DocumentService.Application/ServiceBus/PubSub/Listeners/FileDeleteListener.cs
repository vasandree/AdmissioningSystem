using Common.ServiceBus.RabbitMqMessages.Publish;
using DocumentService.Application.Features.Commands.Documents.DeleteDocument;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.Application.ServiceBus.PubSub.Listeners;

public class FileDeleteListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;


    public FileDeleteListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<DeleteFileMessage>("delete_file_subscription_id", DeleteFile);
    }

    private async Task DeleteFile(DeleteFileMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new DeleteDocumentCommand(message.DocumentType, message.UserId));
        }
    }
}