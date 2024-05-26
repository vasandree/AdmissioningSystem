using AdminPanel.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.Infrastructure.Configurators;

public static class IdentityConfigurator
{
    public static void ConfigureIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<BaseManager, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AdminPanelDbContext>()
            .AddUserManager<UserManager<BaseManager>>()
            .AddSignInManager<SignInManager<BaseManager>>()
            .AddDefaultTokenProviders();
    }
}