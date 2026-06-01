namespace MiniKanban.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync();
}
