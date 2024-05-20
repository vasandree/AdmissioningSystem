using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers.Converters;
using DictionaryService.Persistence.Helpers.Update;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Repositories;

public class ProgramRepository : DictionaryRepository<Program>, IProgramRepository
{
    private readonly DictionaryDbContext _context;
    private readonly ProgramUpdate _update;
    private readonly ProgramConverter _converter;

    public ProgramRepository(DictionaryDbContext context, ProgramUpdate update, ProgramConverter converter) :
        base(context)
    {
        _context = context;
        _update = update;
        _converter = converter;
    }

    public async Task<bool> CheckExistenceByExternalId(Guid externalId)
    {
        return await _context.Programs.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task<List<Program>> GetEntitiesToDelete(IEnumerable<Guid> newIds)
    {
        var oldIds = await _context.Programs.Select(x => x.ExternalId).ToListAsync();

        var idsToDelete = oldIds.Except(newIds);

        return await _context.Programs.Where(x => idsToDelete.Contains(x.ExternalId)).ToListAsync();
    }

    public async Task<Program> GetByExternalId(Guid externalId)
    {
        return await _context.Programs
            .Include(x => x.EducationLevel)
            .Include(x => x.Faculty)
            .FirstOrDefaultAsync(x => x.ExternalId == externalId)!;
    }

    public bool CheckIfChanged(Program program, Program newProgram)
    {
        return  _update.CheckIfProgramUpdated(program, newProgram);
    }

    public async Task UpdateAsync(Program program, Program newProgram)
    {
        await _update.UpdateProgram(program, newProgram);
        _context.Entry(program).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    

    public async Task<List<Program?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel> deletedEducationLevel)
    {
        return await _context.Programs
            .Where(docType => deletedEducationLevel.Any(eduLevel => eduLevel == docType.EducationLevel))
            .ToListAsync();
    }

    public async Task<List<Program?>> GetEntitiesToDeleteByFaculty(List<Faculty> deletedFaculties)
    {
        return await _context.Programs
            .Where(docType => deletedFaculties.Any(faculty => faculty == docType.Faculty))
            .ToListAsync();
    }

    public async Task<Program> Convert(JObject jsonProgram)
    {
        return await _converter.ConvertToProgram(jsonProgram);
    }
}