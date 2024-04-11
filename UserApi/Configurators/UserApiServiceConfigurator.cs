using UserApi.DAL.DbInitializer;

namespace UserApi.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbUserInitializer, DbUserInitializer>(); 
    }
}