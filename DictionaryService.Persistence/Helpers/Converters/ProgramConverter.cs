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
        if (!await _educationLevel.CheckExistenceByExternalId(jsonProgram["educationLevel"]!.Value<int>("id")))
        {
            await _educationLevel.CreateAsync(jsonProgram.Value<JObject>("educationLevel")!);
        }
        var educationLevel = await _educationLevel.GetByExternalId(jsonProgram["educationLevel"]!.Value<int>("id"));

        if (!await _faculty.CheckExistenceByExternalId(Guid.Parse(jsonProgram["faculty"]!.Value<string>("id")!)))
        {
            await _faculty.CreateAsync(jsonProgram.Value<JObject>("faculty")!);
        }
        var faculty = await _faculty.GetByExternalId(Guid.Parse(jsonProgram["faculty"]!.Value<string>("id")!));

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