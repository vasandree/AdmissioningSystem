using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace NotificationApi.Application.Configurators;

public static class NotificationApplicationConfigurator
{
    
    public static void ConfigureNotificationApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBus>(_ =>
        {
            var bus = RabbitHutch.CreateBus("host=localhost");
            bus.Advanced.QueueDeclare("email_queue");
            return bus;
        });
        
        using var serviceProvider = builder.Services.BuildServiceProvider();
        var bus = serviceProvider.GetRequiredService<IBus>();
        
        var subscriber = new AutoSubscriber(bus, "email");
        subscriber.Subscribe(Assembly.GetExecutingAssembly().GetTypes());
    }
}
