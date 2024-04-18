using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;

namespace NotificationApi.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailConfig _emailConfig;

    public SmtpEmailSender(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task<bool> SendEmail(EmailMessage email)
    {
        using (var client = new SmtpClient())
        {
            var emailMessage = new MailMessage()
            {
                From = new MailAddress(_emailConfig.FromAddress, _emailConfig.FromName),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };
            emailMessage.To.Add(email.To);

            client.Host = _emailConfig.SmtpServer;
            client.Port = _emailConfig.SmtpPort;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
            client.EnableSsl = false;

            await client.SendMailAsync(emailMessage);
            return true;
        }
    }
}