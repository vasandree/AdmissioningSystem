using Common.Configurators.Configurator;
using DocumentService.Infrastructure.Configurators;
using DocumentService.Persistence.Configurators;
using DocumentService.Application.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDocumentServiceInfrastructure();

builder.ConfigureDocumentServicePersistence();

builder.ConfigureDocumentServiceApplication();

builder.ConfigureSwagger();

builder.ConfigureServiceBus();

builder.ConfigureAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureDocumentServiceInfrastructure();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.UseMiddleware();

app.Run();
