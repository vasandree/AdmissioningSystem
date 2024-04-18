using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;
using NotificationApi.Infrastructure.Services;

namespace NotificationApi.Infrastructure.Configurators;

public static class NotificationInfrastructureConfigurator
{
    public static void ConfigureNotificationInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
    }
}