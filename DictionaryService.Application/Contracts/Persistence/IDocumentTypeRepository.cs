using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IDocumentTypeRepository : IDictionaryRepository<DocumentType>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);
    
    Task<bool> CheckExistenceById(Guid externalId);

    Task<List<DocumentType>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<DocumentType> GetByExternalId(Guid externalId);

    new Task CreateAsync(DocumentType documentType, List<JObject> jsonNextEducationLevels);

    Task<bool> CheckIfChanged(DocumentType documentType, DocumentType newDocumentType,
        List<JObject> jsonNewNextEducationLevels);

    Task UpdateAsync(DocumentType documentType, DocumentType newDocumentType, List<JObject> jsonNewNextEducationLevels);

    Task<List<DocumentType?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel> deletedEducationLevel);

    new Task<List<DocumentType>> GetAllAsync();

    Task<DocumentType> Convert(JObject jsonDocumentType);
    
    
}