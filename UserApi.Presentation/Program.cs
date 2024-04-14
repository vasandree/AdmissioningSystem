using UserApi.Persistence.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureUserApiService();

builder.ConfigureUserDal();

builder.ConfigureRepositories();

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