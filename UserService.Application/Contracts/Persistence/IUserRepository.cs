using Common.Services.Repository;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task CreateUser(ApplicationUser user, string password);
    Task<ApplicationUser?> GetById(Guid id);
    Task<ApplicationUser?> GetByEmail(string email);
    Task<IList<string>> GetUserRoles(ApplicationUser entity);

    Task ChangePassword(ApplicationUser user,string oldPassword, string newPassword);
    Task<bool> CheckPassword(ApplicationUser user, string password);

    Task ResetPassword(ApplicationUser user, string newPassword);

    Task AddRole(ApplicationUser user, string role);

    Task DeleteRole(ApplicationUser user, string role);
    Task<ApplicationUser> GetAdmin();
}