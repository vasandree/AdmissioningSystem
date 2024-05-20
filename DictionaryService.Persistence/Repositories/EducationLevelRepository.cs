using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers.Converters;
using DictionaryService.Persistence.Helpers.Update;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Repositories;

public class EducationLevelRepository : DictionaryRepository<EducationLevel>, IEducationLevelRepository
{
    private readonly DictionaryDbContext _context;
    private readonly EducationLevelConverter _converterHelper;
    private readonly EducationLevelUpdate _update;


    public EducationLevelRepository(DictionaryDbContext context, EducationLevelConverter converterHelper, EducationLevelUpdate update) : base(context)
    {
        _context = context;
        _converterHelper = converterHelper;
        _update = update;
    }

    public async Task<bool> CheckExistenceByExternalId(int externalId)
    {
        return await _context.EducationLevels.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task<List<EducationLevel>> GetEntitiesToDelete(IEnumerable<int> newIds)
    {
        var oldIds = await _context.EducationLevels.Select(x => x.ExternalId).ToListAsync();

        var idsToDelete = oldIds.Except(newIds);

        return await _context.EducationLevels.Where(x => idsToDelete.Contains(x.ExternalId)).ToListAsync();
    }

    public async Task<EducationLevel> GetByExternalId(int externalId)
    {
        return await _context.EducationLevels.FirstOrDefaultAsync(x => x.ExternalId == externalId)!;
    }

    public new async Task CreateAsync(EducationLevel educationLevel)
    {
        await _context.EducationLevels.AddAsync(educationLevel);
        await _context.SaveChangesAsync();
    }

    public bool CheckIfChanged(EducationLevel educationLevel, EducationLevel newEducationLevel)
    {
        return _update.CheckIfEducationLevelUpdated(educationLevel, newEducationLevel);
    }

    public async Task UpdateAsync(EducationLevel educationLevel, EducationLevel newEducationLevel)
    {
        _update.UpdateEducationLevel(educationLevel, newEducationLevel);
        _context.Entry(educationLevel).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    

    public EducationLevel Convert(JObject jsonEducationLevel)
    {
        return  _converterHelper.ConvertToEducationLevel(jsonEducationLevel);
    }
}