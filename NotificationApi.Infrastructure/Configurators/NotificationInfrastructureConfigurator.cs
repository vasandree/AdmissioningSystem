using Common.Configurators.ConfigClasses;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationApi.Application.Contracts;
using NotificationApi.Infrastructure.PubSubListeners;
using NotificationApi.Infrastructure.Services;

namespace NotificationApi.Infrastructure.Configurators;

public static class NotificationInfrastructureConfigurator
{
    public static void ConfigureNotificationInfrastructure(this WebApplicationBuilder builder)
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>()!; 
        var connectionString = $"host={rabbitMqConfig.Host};username={rabbitMqConfig.UserName};password={rabbitMqConfig.Password};virtualHost={rabbitMqConfig.VirtualHost}"; 

        var bus = RabbitHutch.CreateBus(connectionString); 
        bus.Advanced.QueueDeclare("email_queue");
        builder.Services.AddSingleton<IBus>(bus);

        
        builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
        
        builder.Services.AddHostedService<ResetPasswordCodeListener>();
    }
}