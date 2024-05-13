using System.Reflection;
using Common.ConfigClasses;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Application.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>()!;
        var connectionString =
            $"host={rabbitMqConfig.Host};port={rabbitMqConfig.Port};username={rabbitMqConfig.UserName};password={rabbitMqConfig.Password}";
        var bus = RabbitHutch.CreateBus(connectionString); 
        bus.Advanced.QueueDeclare("email_queue");
        builder.Services.AddSingleton<IBus>(bus);

    }
}