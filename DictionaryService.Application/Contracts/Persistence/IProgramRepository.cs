using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IProgramRepository : IDictionaryRepository<Program>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<List<Program>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<Program> GetByExternalId(Guid externalId);

    bool CheckIfChanged(Program program, Program newProgram);

    Task UpdateAsync(Program program, Program newProgram);

    Task<List<Program?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel> deletedEducationLevel);

    Task<List<Program?>> GetEntitiesToDeleteByFaculty(List<Faculty> deletedFaculties);
    
    Task<Program> Convert(JObject jsonProgram);

    IQueryable<Program> GetAllAsQueryable();
}