using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IFacultyRepository : IDictionaryRepository<Faculty>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<List<Faculty>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<Faculty> GetByExternalId(Guid externalId);
    

    bool CheckIfChanged(Faculty faculty, Faculty newFaculty);

    Task UpdateAsync(Faculty faculty, Faculty newFaculty);
    
    Faculty Convert(JObject jsonFaculty);
}