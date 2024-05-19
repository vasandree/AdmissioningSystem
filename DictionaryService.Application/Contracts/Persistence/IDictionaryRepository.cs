using DictionaryService.Domain.Entities;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IDictionaryRepository<T> : IGenericRepository<T> where T : DictionaryEntity
{
    Task SoftDeleteEntities(List<T> toDelete);

    Task<T> GetById(Guid id);
}