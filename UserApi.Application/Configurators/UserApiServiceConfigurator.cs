using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Application.Contracts.Publishers;


namespace UserApi.Application.Configurators;

public static class UserApiServiceConfigurator
{
    public static void ConfigureUserApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
    }
}