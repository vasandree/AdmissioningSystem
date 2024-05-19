using System.Text.Json.Serialization;

namespace DictionaryService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DictionaryType
{
    All,
    DocumentType, 
    EducationLevel,
    Faculty,
    Program
}