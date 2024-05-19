using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IEducationLevelRepository : IDictionaryRepository<EducationLevel>
{
    Task<bool> CheckExistenceByExternalId(int externalId);

    Task<List<EducationLevel>> GetEntitiesToDelete(IEnumerable<int> newIds);

    Task<EducationLevel> GetByExternalId(int externalId);

    Task CreateAsync(JObject jsonEducationLevel);

    bool CheckIfChanged(EducationLevel educationLevel, JObject jsonEducationLevel);

    Task UpdateAsync(EducationLevel educationLevel, JObject jsonEducationLevel);
}