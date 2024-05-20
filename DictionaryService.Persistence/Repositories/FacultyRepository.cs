using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers.Converters;
using DictionaryService.Persistence.Helpers.Update;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Repositories;

public class FacultyRepository : DictionaryRepository<Faculty>, IFacultyRepository
{
    private readonly DictionaryDbContext _context;
    private readonly FacultyUpdate _update;
    private readonly FacultyConverter _converter;


    public FacultyRepository(DictionaryDbContext context, FacultyUpdate update, FacultyConverter converter) : base(context)
    {
        _context = context;
        _update = update;
        _converter = converter;
    }

    public async Task<bool> CheckExistenceByExternalId(Guid externalId)
    {
        return await _context.Faculties.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task<List<Faculty>> GetEntitiesToDelete(IEnumerable<Guid> newIds)
    {
        var oldIds = await _context.Faculties.Select(x => x.ExternalId).ToListAsync();

        var idsToDelete = oldIds.Except(newIds);

        return await _context.Faculties.Where(x => idsToDelete.Contains(x.ExternalId)).ToListAsync();
    }
    

    public async Task<Faculty> GetByExternalId(Guid externalId)
    {
        return await _context.Faculties.FirstOrDefaultAsync(x => x.ExternalId == externalId)!;
    }

    public new async Task CreateAsync(Faculty newFaculty)
    {
        await _context.Faculties.AddAsync(newFaculty);
        await _context.SaveChangesAsync();
    }

    public bool CheckIfChanged(Faculty faculty, Faculty newFaculty)
    {
        return _update.CheckIfFacultyUpdated(faculty, newFaculty);
    }

    public async Task UpdateAsync(Faculty faculty, Faculty newFaculty)
    {
        _update.UpdateFaculty(faculty, newFaculty);
        _context.Entry(faculty).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public Faculty Convert(JObject jsonFaculty)
    {
        return _converter.ConvertToFaculty(jsonFaculty);
    }

    public async Task<bool> CheckIfExists(Guid id)
    {
        return await _context.Faculties.AnyAsync(x => x.Id == id);
    }
}