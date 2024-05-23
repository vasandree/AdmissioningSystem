using Common.Services.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Common.Configurators.Configurator;

public static class MiddlewareConfigurator
{
    public static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<MiddlewareService>();
    }
}