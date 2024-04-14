using UserApi.Domain.DbEntities;

namespace UserApi.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetById(Guid id);
    Task<ApplicationUser?> GetByEmail(string email);
    Task<IList<string>> GetUserRoles(ApplicationUser entity);
}