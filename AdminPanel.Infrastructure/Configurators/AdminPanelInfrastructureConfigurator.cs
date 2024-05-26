using AdminPanel.Infrastructure.DbInitializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.Infrastructure.Configurators;

public static class AdminPanelInfrastructureConfigurator
{
    public static void ConfigureAdminPanelDb(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresManagers");
        builder.Services.AddDbContext<AdminPanelDbContext>(options => options.UseNpgsql(connection));
        builder.Services.AddScoped<IDbInitializer, DbInitializer.DbInitializer>();
    }

    public static void ConfigureAdminPanelDb(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<AdminPanelDbContext>();
            dbContext?.Database.Migrate();

            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            initializer.InitializeDb();
        }
    }
}