using DictionaryService.Domain.Dictionaries;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Domain.Configurator;

public static class DictionaryServiceDomainConfigurator
{
    public static void ConfigureDictionaryServiceDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<LanguageDictionary>();
        builder.Services.AddScoped<EducationFormDictionary>();
    }
}