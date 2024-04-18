using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NotificationApi.Application.BackgroundService;
using NotificationApi.Application.Сonsumers;

namespace NotificationApi.Application.Configurators;

public static class NotificationApplicationConfigurator
{
    public static void ConfigureNotificationApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IForgetPasswordConsumer, ForgetPasswordConsumer>();
        builder.Services.AddHostedService<NotificationBackgroundService>();
    }
}
