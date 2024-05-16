using System.Linq.Expressions;
using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AdmissionService.Persistence.Repositories;

//TODO: relocate this to common
public class GenericRepository<T> : IGenericRepository<T> where T: class
{
    private readonly AdmissionDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AdmissionDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
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