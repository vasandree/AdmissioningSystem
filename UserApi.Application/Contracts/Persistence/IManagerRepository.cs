using UserApi.Domain.DbEntities;

namespace UserApi.Application.Contracts.Persistence;

public interface IManagerRepository : IGenericRepository<Domain.DbEntities.ManagerEntity>
{
    Task<ManagerEntity?> GetById(Guid id);
    Task<IReadOnlyList<Domain.DbEntities.ManagerEntity>> GetManagers();
    Task<IReadOnlyList<Domain.DbEntities.ManagerEntity>> GetHeadManagers();
    Task<IReadOnlyList<Domain.DbEntities.ManagerEntity>> GetManagersByFaculty(Guid id);
}