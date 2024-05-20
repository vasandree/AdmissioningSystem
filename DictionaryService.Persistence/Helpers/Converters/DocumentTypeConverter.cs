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
        var educationLevel = _educationLevel.Convert(jsonDocumentType.Value<JObject>("educationLevel")!);

        if (!await _educationLevel.CheckExistenceByExternalId(educationLevel.ExternalId))
        {
            await _educationLevel.CreateAsync(educationLevel);
        }

        educationLevel = await _educationLevel.GetByExternalId(educationLevel.ExternalId);

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

        return documentType;
    }
}