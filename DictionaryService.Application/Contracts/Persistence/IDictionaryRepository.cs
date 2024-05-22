using Common.Repository;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IDictionaryRepository<T> : IGenericRepository<T> where T : DictionaryEntity
{
    Task SoftDeleteEntities(List<T> toDelete);

    Task<T> GetById(Guid id);

    new Task<List<T>> GetAllAsync();

    public Task<bool> CheckIfNotEmpty();
}