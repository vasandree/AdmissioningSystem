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

        if (!_roleManager.RoleExistsAsync("Applicant").GetAwaiter().GetResult())
        {
            // Create roles
            _roleManager.CreateAsync(new IdentityRole<Guid>("Applicant")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole<Guid>("Manager")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole<Guid>("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole<Guid>("HeadManager")).GetAwaiter().GetResult();
        }

        // Check if admin user exists
        var adminConfig = _configuration.GetSection("AdminConfig").Get<AdminConfig>();
        var existingAdmin = _userManager.FindByEmailAsync(adminConfig.Email).GetAwaiter().GetResult();
        if (existingAdmin == null)
        {
            // Create admin user
            var adminUser = new ApplicationUser
            {
                FullName = adminConfig.FullName,
                Email = adminConfig.Email,
                UserName = adminConfig.Email // Use email as username for simplicity
            };
            var createUserResult = _userManager.CreateAsync(adminUser, adminConfig.Password).GetAwaiter().GetResult();
            if (createUserResult.Succeeded)
            {
                // Assign "Admin" role to the admin user
                _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
            else
            {
                // Log or handle error if user creation fails
                foreach (var error in createUserResult.Errors)
                {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }
    }
}
