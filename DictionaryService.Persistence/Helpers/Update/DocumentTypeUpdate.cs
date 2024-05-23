using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class DocumentTypeUpdate
{
    private readonly IEducationLevelRepository _educationLevel;
    private readonly EducationLevelUpdate _educationLevelUpdate;
    private readonly INextEducationLevelRepository _nextEducationLevel;

    public DocumentTypeUpdate(IEducationLevelRepository educationLevel, EducationLevelUpdate educationLevelUpdate,
        INextEducationLevelRepository nextEducationLevel)
    {
        _educationLevel = educationLevel;
        _educationLevelUpdate = educationLevelUpdate;
        _nextEducationLevel = nextEducationLevel;
    }

    public async Task<bool> CheckIfDocumentTypeUpdated(DocumentType documentType, DocumentType newDocumentType,
        List<JObject> newNextEducationLevels)
    {
        return documentType.Name != newDocumentType.Name ||
               _educationLevelUpdate.CheckIfEducationLevelUpdated(documentType.EducationLevel,
                   newDocumentType.EducationLevel) ||
               await CheckIfNextEducationLevelsUpdated(documentType.Id, newNextEducationLevels);
    }

    private async Task<bool> CheckIfNextEducationLevelsUpdated(Guid documentTypeId,
        List<JObject> jsonNewNextEducationLevels)
    {
        var oldNextEducationLevels = await _nextEducationLevel.GetEducationLevels(documentTypeId);
        var newNextEducationLevels = jsonNewNextEducationLevels.Select(json => _educationLevel.Convert(json)).ToList();

        var oldIds = oldNextEducationLevels.Select(e => e.ExternalId).ToList();
        var newIds = newNextEducationLevels.Select(e => e.ExternalId).ToList();

        if (!oldIds.SequenceEqual(newIds)) return true;

        foreach (var newNextEducationLevel in newNextEducationLevels)
        {
            var oldNextEducationLevel =
                oldNextEducationLevels.FirstOrDefault(e => e.ExternalId == newNextEducationLevel.ExternalId);
            if (oldNextEducationLevel == null) return true;

            if (_educationLevel.CheckIfChanged(newNextEducationLevel, oldNextEducationLevel))
                return true;
        }

        return false;
    }

    public async Task UpdateDocumentType(DocumentType documentType, DocumentType newDocumentType)
    {
        EducationLevel educationLevel;
        if (!await _educationLevel.CheckExistenceByExternalId(newDocumentType.EducationLevel.ExternalId))
        {
            await _educationLevel.CreateAsync(newDocumentType.EducationLevel);
            educationLevel = await _educationLevel.GetByExternalId(newDocumentType.EducationLevel.ExternalId);
        }
        else
        {
            educationLevel = await _educationLevel.GetByExternalId(newDocumentType.EducationLevel.ExternalId);
            if (_educationLevelUpdate.CheckIfEducationLevelUpdated(educationLevel, newDocumentType.EducationLevel))
                _educationLevelUpdate.UpdateEducationLevel(educationLevel, newDocumentType.EducationLevel);
        }


        documentType.Name = newDocumentType.Name;
        documentType.CreateTime = newDocumentType.CreateTime;
        documentType.EducationLevelId = educationLevel.Id;
        documentType.EducationLevel = educationLevel;
        documentType.IsDeleted = false;
    }


    public async Task CreateNextEducationLevels(DocumentType documentType, List<JObject> jsonNextEducationLevels)
    {
        var oldLevels = await _nextEducationLevel.GetNextEducationLevelsOfDocumentType(documentType.Id);

        if (jsonNextEducationLevels.Count > 0)
        {
            List<NextEducationLevel> addedLevels = new List<NextEducationLevel>();
            foreach (var jsonNextEducationLevel in jsonNextEducationLevels)
            {
                var educationLevel = _educationLevel.Convert(jsonNextEducationLevel);

                if (!await _educationLevel.CheckExistenceByExternalId(educationLevel.ExternalId))
                {
                    await _educationLevel.CreateAsync(educationLevel);
                }
                else
                {
                    var oldEducationLevel = await _educationLevel.GetByExternalId(educationLevel.ExternalId);
                    if (_educationLevelUpdate.CheckIfEducationLevelUpdated(oldEducationLevel, educationLevel))
                        _educationLevelUpdate.UpdateEducationLevel(oldEducationLevel, educationLevel);
                }

                educationLevel = await _educationLevel.GetByExternalId(educationLevel.ExternalId);

                if (!await _nextEducationLevel.CheckIfExists(documentType.Id, educationLevel.ExternalId))
                {
                    var newLevel = new NextEducationLevel
                    {
                        Id = Guid.NewGuid(),
                        EducationLevelId = educationLevel.Id,
                        DocumentTypeId = documentType.Id,
                        EducationLevelExternalId = educationLevel.ExternalId,
                        DocumentTypeExternalId = documentType.ExternalId
                    };
                    await _nextEducationLevel.CreateAsync(newLevel);
                }

                addedLevels.Add(
                    await _nextEducationLevel.GetByExternalIds(documentType.ExternalId, educationLevel.ExternalId));
            }

            if (oldLevels.Count > 0)
            {
                foreach (var oldLevel in oldLevels)
                {
                    if (!addedLevels.Any(al => al.Id == oldLevel!.Id))
                    {
                        await _nextEducationLevel.DeleteAsync(oldLevel!);
                    }
                }
            }
        }
        else
        {
            if (oldLevels.Count > 0)
            {
                foreach (var oldLevel in oldLevels)
                {
                    await _nextEducationLevel.DeleteAsync(oldLevel!);
                }
            }
        }
    }


}