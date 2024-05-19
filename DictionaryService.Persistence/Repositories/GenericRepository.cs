using System.Linq.Expressions;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace DictionaryService.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DictionaryDbContext _context;
    private readonly DbSet<T> _dbSet;


    public GenericRepository(DictionaryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<bool> CheckIfNotEmpty()
    {
        return await _dbSet.AnyAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }
}