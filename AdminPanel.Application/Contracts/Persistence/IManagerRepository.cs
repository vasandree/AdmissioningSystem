
using AdminPanel.Domain.Entities;
using Common.Services.Repository;

namespace AdminPanel.Application.Contracts.Persistence;

public interface IManagerRepository : IGenericRepository<Manager>
{
    public Task<Guid> GetFaculty(Guid mainId);
}