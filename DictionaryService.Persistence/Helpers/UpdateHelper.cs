using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Persistence.Helpers;

public class UpdateHelper
{
    public async void UpdateEducationLevel(EducationLevel educationLevel, EducationLevel existingEducationLevel,
        DictionaryDbContext context)
    {
        var existingName = context.EducationLevels.FirstOrDefault(el => el.Name == educationLevel.Name);

        if (existingName != null)
        {
            existingName.ExternalId = educationLevel.ExternalId;
        }
        else
        {
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateFaculty(Faculty faculty, Faculty existingFaculty, DictionaryDbContext context)
    {
        existingFaculty.Name = faculty.Name;
        existingFaculty.CreateTime = faculty.CreateTime;
        existingFaculty.IsDeleted = false;
        
        context.Entry(existingFaculty).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdateDocumentType(DocumentType documentType, DocumentType existingDocumentType,
        DictionaryDbContext context)
    {
        existingDocumentType.EducationLevel = documentType.EducationLevel;
        existingDocumentType.Name = documentType.Name;
        existingDocumentType.CreateTime = documentType.CreateTime;
        existingDocumentType.EducationLevelId = documentType.EducationLevelId;
        existingDocumentType.NextEducationLevels = documentType.NextEducationLevels;
        existingDocumentType.IsDeleted = false;

        context.Entry(existingDocumentType).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdateProgram(Program program, Program existingProgram, DictionaryDbContext context)
    {
        existingProgram.EducationLevel = program.EducationLevel;
        existingProgram.CreateTime = program.CreateTime;
        existingProgram.Faculty = program.Faculty;
        existingProgram.Code = program.Code;
        existingProgram.Language = program.Language;
        existingProgram.EducationForm = program.EducationForm;
        existingProgram.Name = program.Name;
        existingProgram.IsDeleted = false;

        context.Entry(existingProgram).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}