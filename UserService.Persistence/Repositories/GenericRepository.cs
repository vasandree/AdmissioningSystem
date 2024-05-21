using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Contracts.Persistence;
using UserService.Infrastructure;

namespace UserService.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly UserDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(UserDbContext context)
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