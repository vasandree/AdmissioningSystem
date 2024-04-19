using Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Common.Configurator;

public static class MiddlewareConfigurator
{
    public static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<MiddlewareService>();
    }
}