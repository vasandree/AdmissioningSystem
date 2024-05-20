using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers.Converters;

public class ProgramConverter
{
    private readonly IEducationLevelRepository _educationLevel;
    private readonly IFacultyRepository _faculty;

    public ProgramConverter(IEducationLevelRepository educationLevel, IFacultyRepository faculty)
    {
        _educationLevel = educationLevel;
        _faculty = faculty;
    }

    public async Task<Program> ConvertToProgram(JObject jsonProgram)
    {
        var educationLevel = _educationLevel.Convert(jsonProgram.Value<JObject>("educationLevel")!);
        if (!await _educationLevel.CheckExistenceByExternalId(educationLevel.ExternalId))
        {
            await _educationLevel.CreateAsync(educationLevel);
        }

        educationLevel = await _educationLevel.GetByExternalId(educationLevel.ExternalId);

        var faculty = _faculty.Convert(jsonProgram.Value<JObject>("faculty")!);

        if (!await _faculty.CheckExistenceByExternalId(faculty.ExternalId))
        {
            await _faculty.CreateAsync(faculty);
        }

        faculty = await _faculty.GetByExternalId(faculty.ExternalId);

        return new Program
        {
            Id = Guid.NewGuid(),
            Name = jsonProgram.Value<string>("name")!,
            IsDeleted = false,
            ExternalId = Guid.Parse(jsonProgram.Value<string>("id")!),
            Code = jsonProgram.Value<string>("code")!,
            Language = jsonProgram.Value<string>("language")!,
            EducationForm = jsonProgram.Value<string>("educationForm")!,
            Faculty = faculty,
            EducationLevel = educationLevel,
            CreateTime = jsonProgram.Value<DateTime>("createTime").ToUniversalTime()
        };
    }
}