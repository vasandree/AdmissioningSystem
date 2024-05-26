using AdminPanel.Application.Configurators;
using AdminPanel.Infrastructure.Configurators;
using AdminPanel.Persistence.Configurators;
using Common.Configurators.Configurator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();


builder.ConfigureAdminPanelDb();

builder.ConfigureAdminPanelPersistence();

builder.ConfigureIdentity();

builder.ConfigureAdminPanelApplication();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.ConfigureServiceBus();

var app = builder.Build();

app.ConfigureAdminPanelDb();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();