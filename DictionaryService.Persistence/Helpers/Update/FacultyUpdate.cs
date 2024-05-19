using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class FacultyUpdate
{
    public bool CheckIfFacultyUpdated(Faculty faculty, JObject jsonFaculty)
    {
        return faculty.Name != jsonFaculty.Value<string>("name") ||
               faculty.CreateTime != jsonFaculty.Value<DateTime>("createTime").ToUniversalTime();
    }
    
    public void UpdateFaculty(Faculty faculty, JObject jsonFaculty)
    {
        faculty.CreateTime = jsonFaculty.Value<DateTime>("createTime").ToUniversalTime();
        faculty.Name = jsonFaculty.Value<string>("name")!;
        faculty.IsDeleted = false;
    }

}