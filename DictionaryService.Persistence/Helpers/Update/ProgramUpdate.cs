using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Update;

public class ProgramUpdate
{
    private readonly IEducationLevelRepository _educationLevel;
    private readonly IFacultyRepository _faculty;
    private readonly EducationLevelUpdate _educationLevelUpdate;
    private readonly FacultyUpdate _facultyUpdate;

    public ProgramUpdate(IEducationLevelRepository educationLevel, IFacultyRepository faculty,
        EducationLevelUpdate educationLevelUpdate, FacultyUpdate facultyUpdate)
    {
        _educationLevel = educationLevel;
        _faculty = faculty;
        _educationLevelUpdate = educationLevelUpdate;
        _facultyUpdate = facultyUpdate;
    }

    public bool CheckIfProgramUpdated(Program program, Program newProgram)
    {
        return program.Name != newProgram.Name ||
               program.Code != newProgram.Code ||
               program.Language != newProgram.Language ||
               program.EducationForm != newProgram.EducationForm ||
               _educationLevelUpdate.CheckIfEducationLevelUpdated(program.EducationLevel, newProgram.EducationLevel) ||
               _facultyUpdate.CheckIfFacultyUpdated(program.Faculty, newProgram.Faculty);
    }

    public async Task UpdateProgram(Program program, Program newProgram)
    {
        EducationLevel educationLevel;
        Faculty faculty;

        if (!await _educationLevel.CheckExistenceByExternalId(newProgram.EducationLevel.ExternalId))
        {
            await _educationLevel.CreateAsync(newProgram.EducationLevel);
            educationLevel = await _educationLevel.GetByExternalId(newProgram.EducationLevel.ExternalId);
        }
        else
        {
            educationLevel = await _educationLevel.GetByExternalId(newProgram.EducationLevel.ExternalId);
            if (_educationLevelUpdate.CheckIfEducationLevelUpdated(educationLevel, newProgram.EducationLevel))
            {
                _educationLevelUpdate.UpdateEducationLevel(educationLevel, newProgram.EducationLevel);
                await _educationLevel.UpdateAsync(educationLevel);
            }
            educationLevel = await _educationLevel.GetByExternalId(newProgram.EducationLevel.ExternalId);
        }

        if (!await _faculty.CheckExistenceByExternalId(newProgram.Faculty.ExternalId))
        {
            await _faculty.CreateAsync(newProgram.Faculty);
            faculty = await _faculty.GetByExternalId(newProgram.Faculty.ExternalId);
        }
        else
        {
            faculty = await _faculty.GetByExternalId(newProgram.Faculty.ExternalId);
            if (_facultyUpdate.CheckIfFacultyUpdated(faculty, newProgram.Faculty))
            {
                _facultyUpdate.UpdateFaculty(faculty, newProgram.Faculty);
                await _faculty.UpdateAsync(faculty);
            }
            faculty = await _faculty.GetByExternalId(newProgram.Faculty.ExternalId);
        }
        
        program.Name = newProgram.Name;
        program.Code = newProgram.Code;
        program.Language = newProgram.Language;
        program.EducationForm = newProgram.EducationForm;
        program.CreateTime = newProgram.CreateTime;
        program.EducationLevel = educationLevel;
        program.Faculty = faculty;
    }
}