using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Persistence.Repositories;

public class DocumentRepository<T> : GenericRepository<T>, IDocumentRepository<T> where T : Document
{
    private readonly DbSet<T> _dbSet;

    public DocumentRepository(DocumentsDbContext context) : base(context)
    {
        _dbSet = context.Set<T>();
    }


    public async Task<bool> CheckExistence(Guid userId)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        if (entity == null) return false;
        return true;
    }

    public async Task<Document?> GetByUserId(Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}