using File = DocumentService.Domain.Entities.File;

namespace DocumentService.Application.Contracts.Persistence;

public interface IFileRepository : IGenericRepository<File>
{
    Task<File?> GetById(Guid id);
}
