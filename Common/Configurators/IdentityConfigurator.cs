using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserApi.DAL;
using UserApi.DAL.DbEntities;

namespace Common.Configurators
{
    public static class IdentityConfigurator
    {
        public static void ConfigureIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();
        }
    }
}