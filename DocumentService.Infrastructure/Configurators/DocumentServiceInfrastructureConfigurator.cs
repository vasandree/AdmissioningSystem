using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Infrastructure.Configurators;

public static class DocumentServiceInfrastructureConfigurator
{
    public static void ConfigureDocumentServiceInfrastructure(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresDocuments");
        builder.Services.AddDbContext<DocumentsDbContext>();
    }
    
    public static void ConfigureDocumentServiceInfrastructure(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<DocumentsDbContext>();
            dbContext?.Database.Migrate();
            
        }
    }
}