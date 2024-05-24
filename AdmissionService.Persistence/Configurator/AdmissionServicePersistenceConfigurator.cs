using System.Reflection;
using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Persistence.Repositories;
using Common.Services.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdmissionService.Persistence.Configurator;

public static class AdmissionServicePersistenceConfigurator
{
    public static void ConfigureAdmissionServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();
        builder.Services.AddTransient<IAdmissionRepository, AdmissionRepository>();

    }
}