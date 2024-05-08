using System.Reflection;
using DocumentService.Application.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Application.Configurators;

public static class DocumentServiceConfigurator
{
    public static void ConfigureDocumentServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddTransient<Helper>();
    }
}