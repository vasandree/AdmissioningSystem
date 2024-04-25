using Common.Configurator;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();

builder.ConfigureAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
