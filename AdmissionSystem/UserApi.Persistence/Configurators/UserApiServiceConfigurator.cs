using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Application;
using UserApi.Infrastructure.DbInitializer;

namespace UserApi.Persistence.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddScoped<IDbUserInitializer, DbUserInitializer>(); 
        builder.Services.AddAutoMapper(typeof(MappingProfile));
    }
}