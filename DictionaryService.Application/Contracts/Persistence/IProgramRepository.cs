using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IProgramRepository : IDictionaryRepository<Program>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<List<Program>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<Program> GetByExternalId(Guid externalId);

    Task CreateAsync(JObject jsonProgram);

    Task<bool> CheckIfChanged(Program program, JObject jsonProgram);

    Task UpdateAsync(Program program, JObject jsonProgram);

    Task<List<Program?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel> deletedEducationLevel);

    Task<List<Program?>> GetEntitiesToDeleteByFaculty(List<Faculty> deletedFaculties);
}