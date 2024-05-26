using AdminPanel.Domain.Entities;
using Common.Configurators.ConfigClasses;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdminPanel.Infrastructure.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly AdminPanelDbContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<BaseManager> _userManager;
    private readonly IBus _bus;


    public DbInitializer(AdminPanelDbContext context, RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<BaseManager> userManager, IConfiguration configuration, IBus bus)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
        _bus = bus;
    }

    public void InitializeDb()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any()) _context.Database.Migrate();
        }
        catch (Exception)
        {
            // ignored
        }

        if (!_roleManager.RoleExistsAsync("Manager").GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole<Guid>("Manager")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole<Guid>("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole<Guid>("HeadManager")).GetAwaiter().GetResult();
        }
        
        var adminConfig = _configuration.GetSection("AdminConfig").Get<AdminConfig>();
        var existingAdmin = _userManager.FindByEmailAsync(adminConfig!.Email).GetAwaiter().GetResult();
        if (existingAdmin == null)
        {
            var adminUser = new BaseManager()
            {
                FullName = adminConfig.FullName,
                Email = adminConfig.Email,
                UserName = adminConfig.Email
            };
            
            var createUserResult = _userManager.CreateAsync(adminUser, adminConfig.Password).GetAwaiter().GetResult();
            if (createUserResult.Succeeded)
             
                _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            else
                foreach (var error in createUserResult.Errors)
                    Console.WriteLine($"Error creating admin user: {error.Description}");
        }
    }
}