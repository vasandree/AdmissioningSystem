using NotificationApi.Infrastructure.Configurators;
using NotificationApi.Application.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureNotificationInfrastructure();

builder.ConfigureNotificationApplication();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
