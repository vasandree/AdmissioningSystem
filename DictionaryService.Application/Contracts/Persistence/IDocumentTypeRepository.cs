using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IDocumentTypeRepository : IDictionaryRepository<DocumentType>
{
    Task<bool> CheckExistenceByExternalId(Guid externalId);

    Task<List<DocumentType>> GetEntitiesToDelete(IEnumerable<Guid> newIds);

    Task<DocumentType> GetByExternalId(Guid externalId);

    Task CreateAsync(JObject jsonDocumentType);

    Task<bool> CheckIfChanged(DocumentType documentType, JObject jsonDocumentType);

    Task UpdateAsync(DocumentType documentType, JObject jsonDocumentType);

    Task<List<DocumentType?>> GetEntitiesToDeleteByEducationLevel(List<EducationLevel> deletedEducationLevel);
}