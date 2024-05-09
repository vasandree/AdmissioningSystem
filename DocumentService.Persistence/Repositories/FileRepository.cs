using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Persistence.Repositories;

public class FileRepository : GenericRepository<DbFile>, IFileRepository
{
    private readonly DocumentsDbContext _context;
    
    public FileRepository(DocumentsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<DbFile?> GetById(Guid id)
    {
        return await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
    }
}