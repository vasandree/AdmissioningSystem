using Common.Exceptions;
using Common.Models;
using Common.Models.RabbitMqMessages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.Services;

public class NotificationService : BackgroundService, INotificationService
{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public NotificationService(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<ForgetPasswordMessage>("email_notification_subscription_id", SendForgetPassword, cancellationToken: stoppingToken);
    }

    private async Task SendForgetPassword(ForgetPasswordMessage message)
    {
        try
        {
            await _sender.SendEmail(new EmailMessage
            {
                To = message.Email,
                Subject = "Reset password",
                Body = $"Your reset password code: {message.ConfirmCode}"
            });
        }
        catch (Exception e)
        {
            throw new SmtpServerException(e, message.Email, "forget password code");
        }
    }
}