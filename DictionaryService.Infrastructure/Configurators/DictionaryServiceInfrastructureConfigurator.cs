using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Infrastructure.Configurators;

public static class DictionaryServiceInfrastructureConfigurator
{
    public static void ConfigureDictionaryServiceInfrastructure(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresDictionary");
        builder.Services.AddDbContext<DictionaryDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void ConfigureDictionaryServiceInfrastructure(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<DictionaryDbContext>();
            dbContext?.Database.Migrate();
            
        }
    }
}