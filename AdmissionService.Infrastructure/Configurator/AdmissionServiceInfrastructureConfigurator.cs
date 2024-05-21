using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdmissionService.Infrastructure.Configurator;

public static class AdmissionServiceInfrastructureConfigurator
{
    public static void ConfigureAdmissionServiceInfrastructure(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresAdmission");
        builder.Services.AddDbContext<AdmissionDbContext>(options => options.UseNpgsql(connection));
    }

    public static void ConfigureAdmissionServiceInfrastructure(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<AdmissionDbContext>();
            dbContext?.Database.Migrate();
        }
    }
}