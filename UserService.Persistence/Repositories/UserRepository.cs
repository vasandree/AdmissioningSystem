using Common.Services.Repository;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;
using UserService.Infrastructure;

namespace UserService.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserDbContext context, UserManager<ApplicationUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task CreateUser(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create user: {errors}");
        }
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

    public async Task ChangePassword(ApplicationUser user, string oldPassword, string newPassword)
    {
        await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }

    public async Task<bool> CheckPassword(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task ResetPassword(ApplicationUser user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task AddRole(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task DeleteRole(ApplicationUser user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }
}