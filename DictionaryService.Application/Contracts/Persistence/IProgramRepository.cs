using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IProgramRepository : IDictionaryRepository<Program>
{
    new Task<Program> GetById(Guid id);
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<bool> CheckExistenceById(Guid id);

    Task<List<Program>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<Program> GetByExternalId(Guid externalId);

    bool CheckIfChanged(Program program, Program newProgram);

    Task UpdateAsync(Program program, Program newProgram);

    Task<List<Program?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel?> deletedEducationLevel);

    Task<List<Program?>> GetEntitiesToDeleteByFaculty(List<Faculty> deletedFaculties);
    
    Task<Program> Convert(JObject jsonProgram);

    IQueryable<Program> GetAllAsQueryable();
}