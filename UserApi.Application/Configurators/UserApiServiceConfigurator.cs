using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UserApi.Application.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddSingleton<IBus>(_ =>
        {
            var emailBus = RabbitHutch.CreateBus("host=localhost, ");
            emailBus.Advanced.QueueDeclare( "email_queue", durable: true, exclusive: false, autoDelete: false);
            return emailBus;
        });
        
    }
}