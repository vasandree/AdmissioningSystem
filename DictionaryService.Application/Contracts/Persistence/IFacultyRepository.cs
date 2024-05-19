using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IFacultyRepository : IDictionaryRepository<Faculty>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<List<Faculty>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<Faculty> GetByExternalId(Guid externalId);

    Task CreateAsync(JObject jsonFaculty);

    bool CheckIfChanged(Faculty faculty, JObject jsonFaculty);

    Task UpdateAsync(Faculty faculty, JObject jsonFaculty);
}