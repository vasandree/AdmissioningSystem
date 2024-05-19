using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class EducationLevelUpdate
{
    public bool CheckIfEducationLevelUpdated(EducationLevel educationLevel, JObject jsonEducationLevel)
    {
        return educationLevel.Name != jsonEducationLevel.Value<string>("name");
    }
    
    public void UpdateEducationLevel(EducationLevel educationLevel, JObject jsonEducationLevel)
    {
        educationLevel.Name = jsonEducationLevel.Value<string>("name")!;
        educationLevel.IsDeleted = false;
    }

}