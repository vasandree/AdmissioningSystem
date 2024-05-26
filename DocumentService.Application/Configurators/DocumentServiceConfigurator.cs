using System.Reflection;
using Common.Configurators.ConfigClasses;
using DocumentService.Application.AutoMapper;
using DocumentService.Application.Helpers;
using DocumentService.Application.ServiceBus.PubSub.Listeners;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Application.Configurators;

public static class DocumentServiceConfigurator
{
    public static void ConfigureDocumentServiceApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<Helper>();
        
        builder.Services.AddScoped<RpcRequestSender>();
        builder.Services.AddScoped<PubSubSender>();
        
        builder.Services.AddHostedService<DocumentDeleteListener>();
        builder.Services.AddHostedService<DocumentUpdateListener>();
        builder.Services.AddHostedService<UpdateDocumentInfoListener>();
        builder.Services.AddHostedService<UpdatePassportInfoListener>();
        builder.Services.AddHostedService<FileDeleteListener>();
        builder.Services.AddHostedService<UploadNewFileListener>();
    }
}