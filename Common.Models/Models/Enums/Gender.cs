using System.Text.Json.Serialization;

namespace Common.Models.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}