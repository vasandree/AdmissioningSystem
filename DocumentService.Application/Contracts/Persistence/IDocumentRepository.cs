using Common.Services.Repository;
using DocumentService.Domain.Entities;

namespace DocumentService.Application.Contracts.Persistence;

public interface IDocumentRepository<T> : IGenericRepository<T> where T: Document
{
    Task<bool> CheckExistence(Guid userId);

    Task<Document?> GetByUserId(Guid userId);

    Task SoftDelete(EducationDocument document);

    Task<List<EducationDocument>> GetIdsToDelete(List<Guid> typeIds);
    
}