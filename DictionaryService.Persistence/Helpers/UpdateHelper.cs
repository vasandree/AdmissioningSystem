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
}