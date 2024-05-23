using System.Reflection;
using AdmissionService.Application.Helpers;
using AdmissionService.Application.MappingProfiles;
using AdmissionService.Application.RPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdmissionService.Application.Configurator;

public static class AdmissionServiceApplicationConfigurator
{
    public static void ConfigureAdmissionServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<AdmissionsRearrangeHelper>();

        builder.Services.AddScoped<RpcRequestsSender>();
    }
}