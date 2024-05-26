using AdmissionService.Application.Configurator;
using AdmissionService.Infrastructure.Configurator;
using AdmissionService.Persistence.Configurator;
using Common.Configurators.Configurator;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAdmissionServiceInfrastructure();

builder.ConfigureAdmissionServicePersistence();

builder.ConfigureAdmissionServiceApplication();

builder.ConfigureServiceBus();

builder.ConfigureSwagger();

builder.ConfigureAuth();

builder.AddRPCHandlers();

var app = builder.Build();

app.UseRPCHandlers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware();

app.Run();
