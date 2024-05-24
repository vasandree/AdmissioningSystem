using DocumentService.Application.Helpers;
using DocumentService.Application.RPC.RpcHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Application.Configurators;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<RpcRequestHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<RpcRequestHandler>();
        
        handler.CreateRequestListeners();
    }
}