using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Persistence.Repositories;
using UserApi.Application.Contracts.Persistence;
using UserApi.Persistence.Services;

namespace UserApi.Persistence.Configurators;

public static class RepositoriesConfigurator
{
    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
        builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        
        builder.Services.AddScoped<IJwtService, JwtService>();
    }
}