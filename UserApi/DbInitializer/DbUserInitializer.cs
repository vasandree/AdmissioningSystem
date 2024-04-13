using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserApi.Common.ConfigClasses;
using UserApi.DAL.DbEntities;

namespace UserApi.DAL.DbInitializer;

public class DbUserInitializer : IDbUserInitializer
{
    private readonly UserDbContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;


    public DbUserInitializer(UserDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public void InitializeUserDb()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {
            // ignored
        }
        if (_roleManager.RoleExistsAsync("Applicant").GetAwaiter().GetResult()) return;

        var adminConfig = _configuration.GetSection("AdminConfig").Get<AdminConfig>();

        _roleManager.CreateAsync(new IdentityRole<Guid>("Applicant")).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole<Guid>("Manager")).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole<Guid>("Admin")).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole<Guid>("HeadManager")).GetAwaiter().GetResult();

        _userManager.CreateAsync(new ApplicationUser
        {
            FullName = adminConfig.FullName,
            Email = adminConfig.Email,
            UserName = adminConfig.FullName
        }, adminConfig.Password).GetAwaiter().GetResult();

        var user = _context.Users.FirstOrDefault(u => u.Email == adminConfig.Email);
        if (user != null) _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
    }
}
