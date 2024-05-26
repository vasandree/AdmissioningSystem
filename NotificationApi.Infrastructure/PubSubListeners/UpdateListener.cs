using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class UpdateListener : BackgroundService
{
    
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public UpdateListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdatedToEmailMessage>("email_notification_subscription_id",  SendUpdateEmail);
    } //todo: check

    private async Task SendUpdateEmail(UpdatedToEmailMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Information update",
                Body = message.Message
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "notify about info was updated");
        }
    }
}