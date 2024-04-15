

using UserApi.Persistence.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureUserApiService();

builder.ConfigureUserDb();

builder.ConfigureAuth();

builder.ConfigureIdentity();

builder.ConfigureRepositories();


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

app.Run();