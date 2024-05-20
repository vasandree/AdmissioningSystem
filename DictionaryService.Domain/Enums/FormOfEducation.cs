using System.Text.Json.Serialization;

namespace DictionaryService.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FormOfEducation
{
    Очная,
    ОчноЗаочная
}