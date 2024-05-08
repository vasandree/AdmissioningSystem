using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Persistence.Helpers;
using Microsoft.AspNetCore.Http;

public class Helper
{
    public async Task<Domain.Entities.File> AddFile(IFormFile file, DocumentsDbContext context)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var fileEntity = new Domain.Entities.File()
            {
                Id = new Guid(),
                FileContent = memoryStream.ToArray()
            };

            await context.Files.AddAsync(fileEntity);
            await context.SaveChangesAsync();
            
            return fileEntity;
        }
    }
}