using System.Reflection;
using Common.Configurators.ConfigClasses;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Configurators.Configurator;

public static class ServiceBusConfigurator
{
    public static void ConfigureServiceBus(this WebApplicationBuilder builder)
    {
        
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>()!;
        var connectionString =
            $"host={rabbitMqConfig.Host};port={rabbitMqConfig.Port};username={rabbitMqConfig.UserName};password={rabbitMqConfig.Password}";
        var bus = RabbitHutch.CreateBus(connectionString); 
        builder.Services.AddSingleton(bus);
    }
}