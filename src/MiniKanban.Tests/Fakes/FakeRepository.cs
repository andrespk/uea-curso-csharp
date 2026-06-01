using MiniKanban.Domain.Entities.Abstractions;
using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Tests.Fakes;

public abstract class FakeRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly List<TEntity> Items = new();

    protected FakeRepository()
    {
    }

    protected FakeRepository(IEnumerable<TEntity> items)
    {
        Items.AddRange(items);
    }

    public IReadOnlyList<TEntity> SavedItems => Items;

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(Items.FirstOrDefault(item => item.Id == id));
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TEntity>>(Items.ToList());
    }

    public Task AddAsync(TEntity entity)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        Items.AddRange(entities);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
        var index = Items.FindIndex(item => item.Id == entity.Id);
        if (index >= 0)
            Items[index] = entity;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        Items.RemoveAll(item => item.Id == entity.Id);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        var ids = entities.Select(entity => entity.Id).ToHashSet();
        Items.RemoveAll(item => ids.Contains(item.Id));
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(Items.Any(item => item.Id == id));
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(Items.Count);
    }
}
