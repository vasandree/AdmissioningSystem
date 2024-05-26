using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class EmailToManagerListener : BackgroundService

{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public EmailToManagerListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<EmailToManagerMessage>("manager_appointed_subscription_id", SendEmail);
    }

    private async Task SendEmail(EmailToManagerMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Admission added",
                Body = $"You were appointed as manager to an admission {message.AdmissionId}"
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "forget password code");
        }
    }
}