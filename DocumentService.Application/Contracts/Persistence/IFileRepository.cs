using Common.Services.Repository;
using DocumentService.Domain.Entities;

namespace DocumentService.Application.Contracts.Persistence;

public interface IFileRepository : IGenericRepository<DbFile>
{
    Task<DbFile?> GetById(Guid id);
}
