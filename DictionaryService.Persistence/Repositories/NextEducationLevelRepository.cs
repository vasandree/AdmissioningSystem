using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Persistence.Repositories;

public class NextEducationLevelRepository : GenericRepository<NextEducationLevel>, INextEducationLevelRepository
{
    private readonly DictionaryDbContext _context;
    
    public NextEducationLevelRepository(DictionaryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckIfExists(Guid documentTypeExternalId, int educationLevelExternalId)
    {
        return await _context.NextEducationLevels.AnyAsync(x =>
            x.EducationLevelExternalId == educationLevelExternalId &&
            x.DocumentTypeExternalId == documentTypeExternalId);
    }

    public async Task<NextEducationLevel> GetByExternalIds(Guid documentTypeExternalId, int educationLevelExternalId)
    {
        return await _context.NextEducationLevels.FirstOrDefaultAsync(x =>
            x.EducationLevelExternalId == educationLevelExternalId &&
            x.DocumentTypeExternalId == documentTypeExternalId)!;
    }

    public async Task<List<EducationLevel>> GetEducationLevels(Guid documentTypeId)
    {
        var nextEducationLevels = await _context.NextEducationLevels
            .Where(nel => nel.DocumentTypeId == documentTypeId)
            .ToListAsync();

        var educationLevelIds = nextEducationLevels.Select(nel => nel.EducationLevelId).ToList();

        var educationLevels = await _context.EducationLevels
            .Where(el => educationLevelIds.Contains(el.Id))
            .ToListAsync();

        return educationLevels;
    }

    public async Task<List<NextEducationLevel?>> GetNextEducationLevelsOfDocumentType(Guid documentTypeId)
    {
        return  await _context.NextEducationLevels
            .Where(nel => nel.DocumentTypeId == documentTypeId)
            .ToListAsync()!;
    }
}