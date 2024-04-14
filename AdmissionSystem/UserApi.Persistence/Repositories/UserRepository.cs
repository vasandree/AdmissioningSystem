using Microsoft.AspNetCore.Identity;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;
using UserApi.Infrastructure;

namespace UserApi.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    protected UserRepository(UserDbContext context, UserManager<ApplicationUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetById(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<ApplicationUser?> GetByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IList<string>> GetUserRoles(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}