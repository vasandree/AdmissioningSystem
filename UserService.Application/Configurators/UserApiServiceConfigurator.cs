using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.ServiceBus.PubSub.Listeners;
using UserService.Application.ServiceBus.PubSub.Sender;
using UserService.Application.ServiceBus.RPC.RpcRequestSender;

namespace UserService.Application.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<RpcRequestSender>();
        builder.Services.AddScoped<PubSubSender>();
        
        builder.Services.AddHostedService<UpdateRoleListener>();
        builder.Services.AddHostedService<UpdateUserInfoListener>();
        builder.Services.AddHostedService<UpdatePasswordListener>();
        builder.Services.AddHostedService<UpdateInfoListener>();
        builder.Services.AddHostedService<AddApplicantRoleListener>();
        
    }
}