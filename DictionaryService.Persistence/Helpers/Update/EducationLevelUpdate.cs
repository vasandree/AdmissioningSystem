using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class EducationLevelUpdate
{
    public bool CheckIfEducationLevelUpdated(EducationLevel educationLevel, EducationLevel newEducationLevel)
    {
        return educationLevel.Name != newEducationLevel.Name;
    }

    public void UpdateEducationLevel(EducationLevel educationLevel, EducationLevel newEducationLevel)
    {
        educationLevel.Name = newEducationLevel.Name;
        educationLevel.IsDeleted = false;
    }

}