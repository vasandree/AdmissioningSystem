using DictionaryService.Application.RpcHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.Configurators;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<GetDtosHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        app.Services.GetRequiredService<GetDtosHandler>().CreateRequestListeners();
    }
}