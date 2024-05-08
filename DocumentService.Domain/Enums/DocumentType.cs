using System.Text.Json.Serialization;

namespace DocumentService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentType
{
    Passport,
    EducationDocument
}