using DocumentService.Application.Contracts.Persistence;
using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using File = DocumentService.Domain.Entities.File;

namespace DocumentService.Persistence.Repositories;

public class FileRepository : GenericRepository<File>, IFileRepository
{
    private readonly DocumentsDbContext _context;
    
    public FileRepository(DocumentsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<File?> GetById(Guid id)
    {
        return await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
    }
}