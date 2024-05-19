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

    public ProgramUpdate(IEducationLevelRepository educationLevel, IFacultyRepository faculty, EducationLevelUpdate educationLevelUpdate, FacultyUpdate facultyUpdate)
    {
        _educationLevel = educationLevel;
        _faculty = faculty;
        _educationLevelUpdate = educationLevelUpdate;
        _facultyUpdate = facultyUpdate;
    }

    public async Task<bool> CheckIfProgramUpdated(Program program, JObject jsonProgram)
    {
        return program.Name != jsonProgram.Value<string>("name") ||
               program.CreateTime != jsonProgram.Value<DateTime>("createTime").ToUniversalTime() ||
               program.Code != jsonProgram.Value<string>("code") ||
               program.Language != jsonProgram.Value<string>("language") ||
               program.EducationForm != jsonProgram.Value<string>("educationForm") ||
               program.Faculty !=
               await _faculty.GetByExternalId(Guid.Parse(jsonProgram.Value<string>("faculty:id")!)) ||
               program.EducationLevel !=
               await _educationLevel.GetByExternalId(jsonProgram.Value<int>("educationLevel:id"));
    }
    
    public async Task UpdateProgram(Program program, JObject jsonProgram)
    {
        if (!await _educationLevel.CheckExistenceByExternalId(jsonProgram["educationLevel"]!.Value<int>("id")))
            await _educationLevel.CreateAsync(jsonProgram.Value<JObject>("educationLevel")!);
       
        var educationLevel = await _educationLevel.GetByExternalId(jsonProgram["educationLevel"]!.Value<int>("id"));
        
        if (_educationLevelUpdate.CheckIfEducationLevelUpdated(educationLevel, jsonProgram.Value<JObject>("educationLevel")!))
            _educationLevelUpdate.UpdateEducationLevel(educationLevel, jsonProgram.Value<JObject>("educationLevel")!);
        
        if (!await _faculty.CheckExistenceByExternalId(Guid.Parse(jsonProgram["faculty"]!.Value<string>("id")!)));
        await _faculty.CreateAsync(jsonProgram.Value<JObject>("faculty")!);
       
        var faculty = await _faculty.GetByExternalId(Guid.Parse(jsonProgram["faculty"]!.Value<string>("id")!));
        if (_facultyUpdate.CheckIfFacultyUpdated(faculty, jsonProgram.Value<JObject>("faculty")!))
            _facultyUpdate.UpdateFaculty(faculty, jsonProgram.Value<JObject>("faculty")!);

        program.Name = jsonProgram.Value<string>("name")!;
        program.Code = jsonProgram.Value<string>("code")!;
        program.Language = jsonProgram.Value<string>("language")!;
        program.EducationForm = jsonProgram.Value<string>("educationForm")!;
        program.CreateTime = jsonProgram.Value<DateTime>("createTime").ToUniversalTime();
        program.EducationLevel = educationLevel;
        program.Faculty = faculty;
    }
}