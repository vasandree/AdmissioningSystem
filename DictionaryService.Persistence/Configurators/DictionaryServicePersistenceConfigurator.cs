using DictionaryService.Persistence.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Persistence.Configurators;

public static class DictionaryServicePersistenceConfigurator
{
    public static void ConfigureDictionaryServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ConvertHelper>();
    }
}