using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.RPCHandler;

namespace UserService.Application.Configurators;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<PermissionRpcHandler>();
        builder.Services.AddSingleton<GetUserInfoRpcHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        var permission = app.Services.GetRequiredService<PermissionRpcHandler>();
        permission.CreateRequestListeners();

        var info = app.Services.GetRequiredService<GetUserInfoRpcHandler>();
        info.CreateRequestListeners();
        
    }
}