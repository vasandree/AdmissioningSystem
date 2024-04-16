using NotificationApi.Application.Models;

namespace NotificationApi.Application.Contracts;

public interface IEmailSender
{
    Task<bool> SendEmail(EmailMessage email);
}