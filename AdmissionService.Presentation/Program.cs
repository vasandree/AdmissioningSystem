using AdmissionService.Application.Configurator;
using AdmissionService.Infrastructure.Configurator;
using AdmissionService.Persistence.Configurator;
using Common.Configurators.Configurator;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAdmissionServiceInfrastructure();

builder.ConfigureAdmissionServicePersistence();

builder.ConfigureAdmissionServiceApplication();

builder.ConfigureSwagger();

builder.ConfigureAuth();

var app = builder.Build();

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
