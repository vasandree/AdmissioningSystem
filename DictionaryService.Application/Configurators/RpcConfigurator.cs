using DictionaryService.Application.RpcHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.Configurators;

public static class RpcConfigurator
{
    public static void AddRPCHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ExistanceInDictionaryRpcHandler>();
        builder.Services.AddSingleton<GetDtosHandler>();
    }

    public static void UseRPCHandlers(this WebApplication app)
    {
        var existence = app.Services.GetRequiredService<ExistanceInDictionaryRpcHandler>();
        var getDtos = app.Services.GetRequiredService<GetDtosHandler>();

        existence.CreateRequestListeners();
        getDtos.CreateRequestListeners();
    }
}