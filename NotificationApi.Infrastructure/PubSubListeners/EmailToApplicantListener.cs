using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class EmailToApplicantListener: BackgroundService

{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public EmailToApplicantListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await _bus.PubSub.SubscribeAsync<EmailToApplicantMessage>("manager_appointed_subscription_id", SendEmail);
    }

    private async Task SendEmail(EmailToApplicantMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Manager added",
                Body = $"Manager was appointed on your admission {message.AdmissionId}"
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "forget password code");
        }
    }
}