using Core.Enities;

namespace Core.Interfaces;

public interface IAsyncRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdAsync(string id);
    Task<T?> GetByIdAsync(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<(IReadOnlyList<T?> list, int totalCount)> ListAsync(ISpecification<T> spec);

    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
 
    Task DeleteAsync(T entity);
    Task<int> CountAsync(ISpecification<T> spec);
}
