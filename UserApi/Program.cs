using Common.Configurators;
using UserApi.Configurators;
using UserApi.DAL;
using UserApi.DAL.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureUserApiService();

builder.ConfigureUserDal();

builder.ConfigureIdentity();

var app = builder.Build();

app.ConfigureUserDal();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();