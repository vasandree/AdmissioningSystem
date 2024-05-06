using Common.Exceptions;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers;

public class ConvertHelper
{
    public async Task<DocumentType> ConvertToDocumentType(JObject jsonDocumentType, DictionaryDbContext context)
    {
        var name = jsonDocumentType["name"]!.Value<string>();
        var externalIdString = jsonDocumentType["id"]!.Value<string>();
        var externalId = Guid.Parse(externalIdString!);
        var createTime = jsonDocumentType["createTime"]!.Value<DateTime>().ToUniversalTime();

        var educationLevelExternalIdString = jsonDocumentType["educationLevel"]!["id"].Value<string>();
        var educationLevelExternalId = int.Parse(educationLevelExternalIdString!);
        var educationLevel =
            await context.EducationLevels.FirstOrDefaultAsync(el =>
                el.ExternalId == educationLevelExternalId);

        if (educationLevel == null) throw new BadRequest("Education level not found");

        var documentType = new DocumentType
        {
            ExternalId = externalId,
            Name = name!,
            EducationLevel = educationLevel,
            CreateTime = createTime
        };

        var nextEducationLevels = new List<EducationLevel>();
        var jsonNextEducationLevels = jsonDocumentType["nextEducationLevels"]?.ToObject<List<JObject>>();
        if (jsonNextEducationLevels != null)
        {
            foreach (var jsonNextEducationLevel in jsonNextEducationLevels)
            {
                var nextEducationLevelExternalIdString = jsonNextEducationLevel["id"]!.Value<string>();
                var nextEducationLevelExternalId = int.Parse(nextEducationLevelExternalIdString!);
                var nextEducationLevel =
                    await context.EducationLevels.FirstOrDefaultAsync(el =>
                        el.ExternalId == nextEducationLevelExternalId);
                if (nextEducationLevel != null)
                {
                    nextEducationLevels.Add(nextEducationLevel);
                }
            }
        }

        documentType.NextEducationLevels = nextEducationLevels!;

        return documentType;
    }

    public async Task<Program> ConvertToProgram(JObject jsonProgram, DictionaryDbContext context)
    {
        var externalIdString = jsonProgram["id"]!.Value<string>();
        var externalId = Guid.Parse(externalIdString!);
        var name = jsonProgram["name"]!.Value<string>();
        var code = jsonProgram["code"]!.Value<string>();
        var language = jsonProgram["language"]!.Value<string>();
        var educationForm = jsonProgram["educationForm"]!.Value<string>();
        var createTime = jsonProgram["createTime"]!.Value<DateTime>().ToUniversalTime();

        var facultyExternalIdString = jsonProgram["faculty"]!["id"].Value<string>();
        var facultyExternalId = Guid.Parse(facultyExternalIdString!);
        var faculty = await context.Faculties.FirstOrDefaultAsync(f => f.ExternalId == facultyExternalId);

        var educationLevelExternalIdString = jsonProgram["educationLevel"]!["id"].Value<string>();
        var educationLevelExternalId = int.Parse(educationLevelExternalIdString!);
        var educationLevel =
            await context.EducationLevels.FirstOrDefaultAsync(
                e => e.ExternalId == educationLevelExternalId);

        var program = new Program
        {
            ExternalId = externalId,
            Name = name!,
            Code = code!,
            Language = language!,
            EducationForm = educationForm!,
            Faculty = faculty!,
            EducationLevel = educationLevel!,
            CreateTime = createTime
        };
        return program;
    }

    public Task<EducationLevel> ConvertToEducationKLevel(JObject jsonEducationLevel)
    {
        var id = jsonEducationLevel["id"]!.Value<int>();
        var name = jsonEducationLevel["name"]!.Value<string>();

        return Task.FromResult(new EducationLevel
        {
            ExternalId = id,
            Name = name!
        });
    }

    public Task<Faculty> ConvertToFaculty(JObject jsonFaculty)
    {
        var name = jsonFaculty["name"]!.Value<string>();
        var externalIdString = jsonFaculty["id"]!.Value<string>();
        var externalId = Guid.Parse(externalIdString!);
        var createTime = jsonFaculty["createTime"]!.Value<DateTime>().ToUniversalTime();

        return Task.FromResult(new Faculty
        {
            ExternalId = externalId,
            Name = name!,
            CreateTime = createTime
        });
    }
}