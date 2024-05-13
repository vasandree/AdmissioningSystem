using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.BackgroundServices;
using UserService.Infrastructure.DbInitializer;

namespace UserService.Infrastructure.Configurators;

public static class UserApiDbConfigurator
{
    public static void ConfigureUserDb(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresUser");
        builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));
        builder.Services.AddScoped<IDbUserInitializer, DbUserInitializer>();
        builder.Services.AddHostedService<ExpireRefreshTokensCleanupService>();
    }

    public static void ConfigureUserDb(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<UserDbContext>();
            dbContext?.Database.Migrate();

            var initializer = scope.ServiceProvider.GetRequiredService<IDbUserInitializer>();
            initializer.InitializeUserDb();
        }
    }
}