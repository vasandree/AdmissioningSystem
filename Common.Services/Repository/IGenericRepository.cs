using System.Linq.Expressions;

namespace Common.Services.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> expression);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    Task CreateAsync(T entity);
}
