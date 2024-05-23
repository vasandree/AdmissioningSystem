using Common.Services.Repository;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Persistence.Repositories;

public class DocumentRepository<T> : GenericRepository<T>, IDocumentRepository<T> where T : Document
{
    private readonly DbSet<T> _dbSet;
    private readonly DocumentsDbContext _context;

    public DocumentRepository(DocumentsDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }


    public Task<bool> CheckExistence(Guid userId)
    {
        return Task.FromResult(_dbSet.Any(x => x.UserId == userId && !x.IsDeleted));
    }

    public async Task<Document?> GetByUserId(Guid userId)
    {
        return await _dbSet.Include(p => p.File).FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task SoftDelete(EducationDocument document)
    {
        document.IsDeleted = true;
        _context.Entry(document).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
    }

    public async Task<List<EducationDocument>> GetIdsToDelete(List<Guid> typeIds)
    {
        var documentIds = await _context.EducationDocuments
            .Where(doc => typeIds.Contains(doc.EducationDocumentTypeId ?? Guid.Empty) && !doc.IsDeleted)
            .ToListAsync();

        return documentIds;
    }
}