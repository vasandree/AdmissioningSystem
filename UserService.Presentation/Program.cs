using Common.Configurator;
using UserService.Application.Configurators;
using UserService.Infrastructure.Configurators;
using UserService.Persistence.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureUserApiService();

builder.ConfigureUserDb();

builder.ConfigureAuth();

builder.ConfigureIdentity();

builder.ConfigureRepositories();

builder.ConfigureSwagger();

var app = builder.Build();

app.ConfigureUserDb();

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