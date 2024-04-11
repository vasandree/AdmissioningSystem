using Common.Configurators;
using UserApi.Configurators;
using UserApi.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureUserDal();

builder.ConfigureUserApiService();

builder.ConfigureIdentity();

var app = builder.Build();

app.ConfigureUserDal();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();