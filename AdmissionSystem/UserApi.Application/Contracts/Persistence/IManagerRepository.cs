using UserApi.Domain.DbEntities;

namespace UserApi.Application.Contracts.Persistence;

public interface IManagerRepository : IGenericRepository<ManagerEntity>
{
    Task<ManagerEntity?> GetById(Guid id);
    Task<IReadOnlyList<ManagerEntity>> GetManagers();
    Task<IReadOnlyList<ManagerEntity>> GetHeadManagers();
    Task<IReadOnlyList<ManagerEntity>> GetManagersByFaculty(Guid id);
}