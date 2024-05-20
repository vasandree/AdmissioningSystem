using DictionaryService.Domain.Entities;

namespace DictionaryService.Application.Contracts.Persistence;

public interface INextEducationLevelRepository : IGenericRepository<NextEducationLevel>
{
    Task<bool> CheckIfExists(Guid documentTypeExternalId, int educationLevelExternalId);

    Task<NextEducationLevel> GetByExternalIds(Guid documentTypeExternalId, int educationLevelExternalId);

    Task<List<EducationLevel>> GetEducationLevels(Guid documentTypeId);

    Task<List<NextEducationLevel?>> GetNextEducationLevelsOfDocumentType(Guid documentTypeId);
}