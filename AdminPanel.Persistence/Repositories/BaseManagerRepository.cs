using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure;
using Common.Services.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Persistence.Repositories;

public class BaseManagerRepository : GenericRepository<BaseManager>, IBaseManagerRepository
{
    private readonly UserManager<BaseManager> _userManager;

    public BaseManagerRepository(AdminPanelDbContext context, UserManager<BaseManager> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task<bool> CheckIfManager(BaseManager manager)
    {
        var roles = await _userManager.GetRolesAsync(manager);
        return roles.Contains("Manager");
    }

    public async Task<BaseManager> GetById(Guid? managerId)
    {
        return (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == managerId))!;
    }

    public Task<bool> CheckExistence(Guid managerId)
    {
        return _userManager.Users.AnyAsync(x => x.Id == managerId);
    }

    public async Task<BaseManager> GetByEmail(string email)
    {
        return (await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email))!;
    }
}