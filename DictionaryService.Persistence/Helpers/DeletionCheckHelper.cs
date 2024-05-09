using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Persistence.Helpers;

public class DeletionCheckHelper
{
    public async void EducationLevelDeletionCheck(List<JObject> jsonEducationLevels, DictionaryDbContext context)
    {
    }

    public async Task FacultiesDeletionCheck(List<JObject>? jsonFaculties, DictionaryDbContext context)
    {
        var dbFaculties = await context.Faculties.ToListAsync();

        foreach (var dbFaculty in dbFaculties)
        {
            var existsInJson = jsonFaculties.Any(jf => Guid.Parse(jf["id"]!.ToString()) == dbFaculty.ExternalId);

            if (!existsInJson)
            {
                dbFaculty.IsDeleted = true;
                await DeleteProgramsByFaculty(dbFaculty.ExternalId, context);
            }
        }

        await context.SaveChangesAsync();
    }

    private async Task DeleteProgramsByFaculty(Guid dbFacultyExternalId, DictionaryDbContext context)
    {
        var programs = await context.Programs
            .Where(p => p.Faculty.ExternalId == dbFacultyExternalId)
            .ToListAsync();
        
        foreach (var program in programs)
        {
            program.IsDeleted = true;
        }
        
        await context.SaveChangesAsync();
    }

    public async Task ProgramsDeletionCheck(List<JObject>? jsonPrograms, DictionaryDbContext context)
    {
        var dbPrograms = await context.Programs.ToListAsync();

        foreach (var dbProgram in dbPrograms)
        {
            var existsInJson = jsonPrograms.Any(jf => Guid.Parse(jf["id"]!.ToString()) == dbProgram.ExternalId);

            if (!existsInJson)
            {
                dbProgram.IsDeleted = true;
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task DocumentTypesDeletionCheck(List<JObject>? jsonDocumentTypes, DictionaryDbContext context)
    {
        var dbFdDocumentTypes = await context.DocumentTypes.ToListAsync();

        foreach (var dbDocumentType in dbFdDocumentTypes)
        {
            var existsInJson =
                jsonDocumentTypes.Any(jf => Guid.Parse(jf["id"]!.ToString()) == dbDocumentType.ExternalId);

            if (!existsInJson)
            {
                dbDocumentType.IsDeleted = true;
            }
        }

        await context.SaveChangesAsync();
    }
}