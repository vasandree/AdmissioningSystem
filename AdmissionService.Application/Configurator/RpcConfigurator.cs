using AdmissionService.Application.ServiceBus.RPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdmissionService.Application.Configurator;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<RpcHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        app.Services.GetRequiredService<RpcHandler>().CreateRequestListeners();
    }
}