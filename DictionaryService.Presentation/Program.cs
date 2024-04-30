using Common.Configurator;
using DictionaryService.Application.Configurators;
using DictionaryService.Infrastructure.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAuth();

builder.ConfigureDictionaryServiceInfrastructure();

builder.ConfigureDictionaryApplicationService();

builder.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureDictionaryServiceInfrastructure();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware();

app.Run();