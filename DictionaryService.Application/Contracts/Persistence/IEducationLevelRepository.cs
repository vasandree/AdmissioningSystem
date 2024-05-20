using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IEducationLevelRepository : IDictionaryRepository<EducationLevel>
{
    Task<bool> CheckExistenceByExternalId(int externalId);

    Task<List<EducationLevel>> GetEntitiesToDelete(IEnumerable<int> newIds);

    Task<EducationLevel> GetByExternalId(int externalId);

    bool CheckIfChanged(EducationLevel educationLevel, EducationLevel newEducationLevel);

    Task UpdateAsync(EducationLevel educationLevel, EducationLevel newEducationLevel);
    
    EducationLevel Convert(JObject jsonEducationLevel);
}