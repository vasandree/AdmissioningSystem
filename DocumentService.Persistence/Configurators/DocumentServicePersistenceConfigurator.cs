using DocumentService.Persistence.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Persistence.Configurators;

public static class DocumentServicePersistenceConfigurator
{
    public static void ConfigureDocumentServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Helper>();
    }
}