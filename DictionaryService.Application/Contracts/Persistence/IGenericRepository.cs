using System.Linq.Expressions;

namespace DictionaryService.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<bool> CheckIfNotEmpty();
    
    Task<IReadOnlyList<T>> GetAllAsync();
    
    Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> expression);
    
    Task UpdateAsync(T entity);
    
    Task DeleteAsync(T entity);

    Task CreateAsync(T entity);
}