using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Converters;

public class EducationLevelConverter
{
    public EducationLevel ConvertToEducationLevel(JObject jsonEducationLevel)
    {
        return new EducationLevel
        {
            Id = Guid.NewGuid(),
            Name = jsonEducationLevel.Value<string>("name")!,
            ExternalId = jsonEducationLevel.Value<int>("id")!,
            IsDeleted = false
        };
    }
}