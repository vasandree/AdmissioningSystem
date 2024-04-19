using NotificationApi.Infrastructure.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureNotificationInfrastructure();

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
