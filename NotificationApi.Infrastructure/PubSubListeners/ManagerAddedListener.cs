using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class ManagerAddedListener : BackgroundService

{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public ManagerAddedListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ManagerAddedMessage>("manager_added_subscription_id", SendEmail);
    }

    private async Task SendEmail(ManagerAddedMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Added role",
                Body = $"You were appointed as {message.Role}" + 
                       (string.IsNullOrEmpty(message.FacultyId.ToString()) ? "" : $" for faculty {message.FacultyId}")
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "forget password code");
        }
    }
}
