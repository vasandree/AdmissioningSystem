using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class DocumentTypeUpdate
{
    private readonly IEducationLevelRepository _educationLevel;
    private readonly EducationLevelUpdate _educationLevelUpdate;

    public DocumentTypeUpdate(IEducationLevelRepository educationLevel, EducationLevelUpdate educationLevelUpdate)
    {
        _educationLevel = educationLevel;
        _educationLevelUpdate = educationLevelUpdate;
    }

    public async Task<bool> CheckIfDocumentTypeUpdated(DocumentType documentType, JObject jsonDocumentType)
    {
        return documentType.Name != jsonDocumentType.Value<string>("name") ||
               documentType.CreateTime != jsonDocumentType.Value<DateTime>("createTime").ToUniversalTime() ||
               documentType.EducationLevel !=
               await _educationLevel.GetByExternalId(jsonDocumentType.Value<int>("educationLevel:id")) ||
               !await CheckNextEducationLevels(documentType,
                   jsonDocumentType["nextEducationLevels"]!.ToObject<List<JObject>>()!);
    }
    
    public async Task UpdateDocumentType(DocumentType documentType, JObject jsonDocumentType)
    {
        if (!await _educationLevel.CheckExistenceByExternalId(jsonDocumentType.Value<int>("educationLevel:id")))
            await _educationLevel.CreateAsync(jsonDocumentType.Value<JObject>("educationLevel")!);
       
        var educationLevel = await _educationLevel.GetByExternalId(jsonDocumentType.Value<int>("educationLevel:id"));
        if (_educationLevelUpdate.CheckIfEducationLevelUpdated(educationLevel, jsonDocumentType.Value<JObject>("educationLevel")!))
            _educationLevelUpdate.UpdateEducationLevel(educationLevel, jsonDocumentType.Value<JObject>("educationLevel")!);
        

        documentType.Name = jsonDocumentType.Value<string>("name")!;
        documentType.CreateTime = jsonDocumentType.Value<DateTime>("createTime").ToUniversalTime();
        documentType.EducationLevelId = educationLevel.Id;
        documentType.EducationLevel = educationLevel;
        documentType.IsDeleted = false;

        await UpdateNextEducationLevels(documentType,
            jsonDocumentType["nextEducationLevels"]!.ToObject<List<JObject>>()!);
    }
    
    private async Task<bool> CheckNextEducationLevels(DocumentType documentType, List<JObject> nextEducationLevels)
    {
        var oldNextEducationLevels = documentType.NextEducationLevels;

        var newNextEducationLevels = await GetNextEducationLevels(nextEducationLevels);
        var upToDate = oldNextEducationLevels.SequenceEqual(newNextEducationLevels);
        return upToDate;
    }

    private async Task<List<EducationLevel>> GetNextEducationLevels(List<JObject> nextEducationLevels)
    {
        var newNextEducationLevels = new List<EducationLevel>();
        foreach (var next in nextEducationLevels)
        {
            if (!await _educationLevel.CheckExistenceByExternalId(next.Value<int>("id")))
            {
                await _educationLevel.CreateAsync(next);
            }
            newNextEducationLevels.Add(await _educationLevel.GetByExternalId(next.Value<int>("id")));
        }

        return newNextEducationLevels;
    }

    private async Task UpdateNextEducationLevels(DocumentType documentType, List<JObject> newNextLevels)
    {
        if (!await CheckNextEducationLevels(documentType, newNextLevels))
        {
            documentType.NextEducationLevels = await GetNextEducationLevels(newNextLevels);
        }
    }
}