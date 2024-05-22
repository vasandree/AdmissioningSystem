using Common.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.Persistence;
using UserService.Persistence.Repositories;
using UserService.Persistence.Services;

namespace UserService.Persistence.Configurators;

public static class RepositoriesConfigurator
{
    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
        builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<ITokenRepository, TokenRepository>();
        
        builder.Services.AddScoped<IJwtService, JwtService>(); 
    }
}