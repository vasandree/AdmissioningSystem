using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.PubSubListeners;

public class ResetPasswordCodeListener : BackgroundService
{
    private readonly IBus _bus;
    private readonly IEmailSender _sender;

    public ResetPasswordCodeListener(IBus bus, IEmailSender sender)
    {
        _bus = bus;
        _sender = sender;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<ForgetPasswordMessage>("email_notification_subscription_id",  SendForgetPassword);
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