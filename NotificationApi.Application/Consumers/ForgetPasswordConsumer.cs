using Common.Models;
using EasyNetQ.AutoSubscribe;
using NotificationApi.Application.Models;
using IEmailSender = NotificationApi.Application.Contracts.IEmailSender;

namespace NotificationApi.Application.Consumers;

public class ForgetPasswordConsumer : IConsume<ForgetPasswordMessage>
{
    private readonly IEmailSender _sender;
    
    public ForgetPasswordConsumer(IEmailSender sender)
    {
        _sender = sender;
    }


    public async void Consume(ForgetPasswordMessage message, CancellationToken cancellationToken = new CancellationToken())
    {
        var emailMessage = new EmailMessage
        {
            To = message.Email,
            Subject = "Reset password",
            Body = $"Your reset password code: {message.ConfirmCode}"
        };
        await _sender.SendEmail(emailMessage);
    }
}