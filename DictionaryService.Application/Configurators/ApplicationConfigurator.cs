using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.Configurators;

public static class ApplicationConfigurator
{
    public static void ConfigureDictionaryApplicationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddHttpClient();
    }
}