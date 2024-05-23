using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers.Converters;
using DictionaryService.Persistence.Helpers.Update;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Repositories;

public class DocumentTypeRepository : DictionaryRepository<DocumentType>, IDocumentTypeRepository
{
    private readonly DictionaryDbContext _context;
    private readonly DocumentTypeUpdate _update;
    private readonly DocumentTypeConverter _converter;

    public DocumentTypeRepository(DictionaryDbContext context, DocumentTypeUpdate update,
        DocumentTypeConverter converter) : base(context)
    {
        _context = context;
        _update = update;
        _converter = converter;
    }

    public new async Task<DocumentType> GetById(Guid id)
    {
        return await _context.DocumentTypes
            .Include(x => x.EducationLevel)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)!;
    }
    
    public async Task<bool> CheckExistenceByExternalId(Guid externalId)
    {
        return await _context.DocumentTypes.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task<bool> CheckExistenceById(Guid id)
    {
        return await _context.DocumentTypes.AnyAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<DocumentType>> GetEntitiesToDelete(IEnumerable<Guid> newIds)
    {
        var oldIds = await _context.DocumentTypes.Select(x => x.ExternalId).ToListAsync();

        var idsToDelete = oldIds.Except(newIds);

        return await _context.DocumentTypes.Where(x => idsToDelete.Contains(x.ExternalId)).ToListAsync();
    }

    public async Task<DocumentType> GetByExternalId(Guid externalId)
    {
        return (await _context.DocumentTypes
            .Include(x => x.EducationLevel)
            .FirstOrDefaultAsync(x => x.ExternalId == externalId))!;
    }

    public new async Task CreateAsync(DocumentType documentType, List<JObject> jsonNextEducationLevels)
    {
        await _context.DocumentTypes.AddAsync(documentType);
        await _context.SaveChangesAsync();

        await _update.CreateNextEducationLevels(documentType, jsonNextEducationLevels);
    }

    public async Task<bool> CheckIfChanged(DocumentType documentType, DocumentType newDocumentType, List<JObject> jsonNewNextEducationLevels)
    {
        return await _update.CheckIfDocumentTypeUpdated(documentType, newDocumentType, jsonNewNextEducationLevels);
    }


    public async Task UpdateAsync(DocumentType documentType, DocumentType newDocumentType, List<JObject> jsonNewNextEducationLevels)
    {
        await _update.UpdateDocumentType(documentType, newDocumentType);
        _context.Entry(documentType).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        await _update.CreateNextEducationLevels(documentType, jsonNewNextEducationLevels);
    }

    public async Task<List<DocumentType?>> GetEntitiesToDeleteByEducationLevel(
        List<EducationLevel> deletedEducationLevel)
    {
        var documentTypes = await _context.DocumentTypes.ToListAsync();

        List<DocumentType> entitiesToDelete = documentTypes
            .Where(docType => deletedEducationLevel.Any(eduLevel => eduLevel.Id == docType.EducationLevelId))
            .ToList();

        return entitiesToDelete;
    }

    public new async Task<List<DocumentType>> GetAllAsync()
    {
        return await _context.DocumentTypes
            .Where(x => !x.IsDeleted)
            .Include(x => x.EducationLevel)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DocumentType> Convert(JObject jsonDocumentType)
    {
        return await _converter.ConvertToDocumentType(jsonDocumentType);
    }
}