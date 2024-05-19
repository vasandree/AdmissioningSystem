using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Converters;

public class DocumentTypeConverter
{
    private readonly IEducationLevelRepository _educationLevel;

    public DocumentTypeConverter(IEducationLevelRepository educationLevel)
    {
        _educationLevel = educationLevel;
    }

    public async Task<DocumentType> ConvertToDocumentType(JObject jsonDocumentType)
    {
        if (!await _educationLevel.CheckExistenceByExternalId(jsonDocumentType["educationLevel"]!.Value<int>("id")))
        {
            await _educationLevel.CreateAsync(jsonDocumentType.Value<JObject>("educationLevel")!);
        }
        
        var educationLevel = await _educationLevel.GetByExternalId(jsonDocumentType["educationLevel"]!.Value<int>("id"));

        var documentType = new DocumentType
        {
            Id = Guid.NewGuid(),
            Name = jsonDocumentType.Value<string>("name")!,
            IsDeleted = false,
            ExternalId = Guid.Parse(jsonDocumentType.Value<string>("id")!),
            EducationLevelId = educationLevel.Id,
            EducationLevel = educationLevel,
            CreateTime = jsonDocumentType.Value<DateTime>("createTime").ToUniversalTime(),
        };

        var jsonNextEducationLevels = jsonDocumentType["nextEducationLevels"]?.ToObject<List<JObject>>();
        if (jsonNextEducationLevels.Count > 0)
        {
            documentType.NextEducationLevels = await ConvertNextEducationLevels(jsonNextEducationLevels);
        }

        return documentType;
    }

    private async Task<List<EducationLevel>> ConvertNextEducationLevels(List<JObject> jsonNextEducationLevels)
    {
        var nextEducationLevels = new List<EducationLevel>();
        foreach (var jsonNextEducationLevel in jsonNextEducationLevels)
        {
            var nextEducationLevelExternalId = jsonNextEducationLevel.Value<int>("id");
            if (!await _educationLevel.CheckExistenceByExternalId(nextEducationLevelExternalId))
            {
                await _educationLevel.CreateAsync(jsonNextEducationLevel);
            }
            var educationLevel = await _educationLevel.GetByExternalId(nextEducationLevelExternalId);
            nextEducationLevels.Add(educationLevel);
        }
        return nextEducationLevels;
    }
}