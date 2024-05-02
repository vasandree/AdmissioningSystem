using DocumentService.Infrastructure.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDocumentServiceInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureDocumentServiceInfrastructure();

app.UseHttpsRedirection();

app.Run();
