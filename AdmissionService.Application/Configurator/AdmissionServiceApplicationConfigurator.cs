using System.Reflection;
using AdmissionService.Application.Helpers;
using AdmissionService.Application.MappingProfiles;
using AdmissionService.Application.ServiceBus.PubSub.Listeners;
using AdmissionService.Application.ServiceBus.PubSub.Senders;
using AdmissionService.Application.ServiceBus.RPC;
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
        builder.Services.AddScoped<PubSubSender>();

        builder.Services.AddHostedService<ProgramDeletionListener>();
        builder.Services.AddHostedService<ProgramUpdateListener>();
        builder.Services.AddHostedService<AdmissionDeleteListener>();
        builder.Services.AddHostedService<AdmissionPriorityChangeListener>();
        builder.Services.AddHostedService<AdmissionStatusUpdateListener>();
        builder.Services.AddHostedService<DeleteManagerFromAdmissionListener>();
        builder.Services.AddHostedService<UpdateStatusesListener>();
        builder.Services.AddHostedService<GetApplicantListener>();
    }
}