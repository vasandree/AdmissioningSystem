using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers;
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

    public DocumentTypeRepository(DictionaryDbContext context, DocumentTypeUpdate update, DocumentTypeConverter converter) : base(context)
    {
        _context = context;
        _update = update;
        _converter = converter;
    }


    public async Task<bool> CheckExistenceByExternalId(Guid externalId)
    {
        return await _context.DocumentTypes.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task<List<DocumentType>> GetEntitiesToDelete(IEnumerable<Guid> newIds)
    {
        var oldIds = await _context.DocumentTypes.Select(x => x.ExternalId).ToListAsync();

        var idsToDelete = oldIds.Except(newIds);

        return await _context.DocumentTypes.Where(x => idsToDelete.Contains(x.ExternalId)).ToListAsync();
    }

    public async Task<DocumentType> GetByExternalId(Guid externalId)
    {
        return await _context.DocumentTypes.FirstOrDefaultAsync(x => x.ExternalId == externalId)!;
    }

    public async Task CreateAsync(JObject jsonDocumentType)
    {
        await _context.DocumentTypes.AddAsync(await _converter.ConvertToDocumentType(jsonDocumentType));
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfChanged(DocumentType documentType, JObject jsonDocumentType)
    {
        return await _update.CheckIfDocumentTypeUpdated(documentType, jsonDocumentType);
    }

    public async Task UpdateAsync(DocumentType documentType, JObject jsonDocumentType)
    {
        await _update.UpdateDocumentType(documentType, jsonDocumentType);
        _context.Entry(documentType).State = EntityState.Modified;
        await _context.SaveChangesAsync();
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
}