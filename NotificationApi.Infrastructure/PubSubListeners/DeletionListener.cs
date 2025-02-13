using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class DeletionListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public DeletionListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<DeletedToEmailMessage>("email_deletion_subscription_id", SendDeleted);
    } //todo: check

    private async Task SendDeleted(DeletedToEmailMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Deletion",
                Body = message.Message
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "notify about deletion");
        }
    }
}