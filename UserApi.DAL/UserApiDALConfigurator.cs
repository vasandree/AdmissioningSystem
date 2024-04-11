using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApi.DAL.DbInitializer;

namespace UserApi.DAL;

public static class UserApiDalConfigurator
{
    public static void ConfigureUserDal(this WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("PostgresUser");
        builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));
        
    }

    public static void ConfigureUserDal(this WebApplication application)
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