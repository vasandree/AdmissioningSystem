using AdminPanel.Domain.Entities;
using Common.Services.Repository;

namespace AdminPanel.Application.Contracts.Persistence;

public interface IBaseManagerRepository : IGenericRepository<BaseManager>
{
    Task<bool> CheckIfManager(BaseManager manager);
    Task<BaseManager> GetById(Guid? managerId);
    Task<bool> CheckExistence(Guid requestManagerId);

    Task<BaseManager> GetByEmail(string email);
}