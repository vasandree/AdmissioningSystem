using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Converters;

public class FacultyConverter
{
    public Faculty ConvertToFaculty(JObject jsonFaculty)
    {
        return new Faculty
        {
            Id = Guid.NewGuid(),
            Name = jsonFaculty.Value<string>("name")!,
            IsDeleted = false,
            ExternalId = Guid.Parse(jsonFaculty.Value<string>("id")!),
            CreateTime = jsonFaculty.Value<DateTime>("createTime").ToUniversalTime()
        };
    }
}