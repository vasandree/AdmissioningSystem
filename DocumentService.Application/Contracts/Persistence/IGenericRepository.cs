using System.Linq.Expressions;

namespace DocumentService.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> expression);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task CreateAsync(T entity);
    
}