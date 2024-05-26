using System.Reflection;
using AdminPanel.Application.ServiceBus.PubSub.Listeners;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.Application.Configurators;

public static class AdminPanelApplicationConfigurator
{
    public static void ConfigureAdminPanelApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<RpcRequestSender>();
        builder.Services.AddScoped<PubSubSender>();

        builder.Services.AddHostedService<AdmissionListener>();
    }
}