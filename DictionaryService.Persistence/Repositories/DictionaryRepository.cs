using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Persistence.Repositories;

public class DictionaryRepository<T> : GenericRepository<T>, IDictionaryRepository<T> where T : DictionaryEntity
{
    private readonly DictionaryDbContext _context;

    public DictionaryRepository(DictionaryDbContext context) : base(context)
    {
        _context = context;
    }
    

    public async Task SoftDeleteEntities(List<T> toDelete)
    {
        foreach (var entity in toDelete)
        {
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
    

    public async Task<T> GetById(Guid id)
    {
        return (await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id))!;
    }
}