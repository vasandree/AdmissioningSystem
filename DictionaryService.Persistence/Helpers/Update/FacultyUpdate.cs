using DictionaryService.Domain.Entities;

namespace DictionaryService.Persistence.Helpers.Update;

public class FacultyUpdate
{
    public bool CheckIfFacultyUpdated(Faculty faculty, Faculty newFaculty)
    {
        return faculty.Name != newFaculty.Name;
    }
    
    public void UpdateFaculty(Faculty faculty, Faculty newFaculty)
    {
        faculty.CreateTime = newFaculty.CreateTime;
        faculty.Name = newFaculty.Name!;
        faculty.IsDeleted = false;
    }

}