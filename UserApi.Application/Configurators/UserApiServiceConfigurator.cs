using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Infrastructure.DbInitializer;

namespace UserApi.Application.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbUserInitializer, DbUserInitializer>(); 
    }
}