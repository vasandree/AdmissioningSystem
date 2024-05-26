using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Persistence.Repositories;
using Common.Services.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.Persistence.Configurators;

public static class AdminPanelPersistenceConfigurator
{
    public static void ConfigureAdminPanelPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<IAdmissionRepository, AdmissionRepository>();
        builder.Services.AddTransient<IBaseManagerRepository, BaseManagerRepository>();
        builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
        builder.Services.AddTransient<IHeadManagerRepository, HeadManagerRepository>();
    }
}