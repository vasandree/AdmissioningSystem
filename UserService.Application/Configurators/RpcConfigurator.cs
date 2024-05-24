using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.RPCHandler;

namespace UserService.Application.Configurators;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<UserServiceRpcHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        app.Services.GetRequiredService<UserServiceRpcHandler>().CreateRequestListeners();
    }
}