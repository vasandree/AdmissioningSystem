using System.Text.Json.Serialization;

namespace UserApi.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}