using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Tests.Fakes;

public class FakeKanbanColumnRepository : FakeRepository<KanbanColumn>, IKanbanColumnRepository
{
    public FakeKanbanColumnRepository()
    {
    }

    public FakeKanbanColumnRepository(IEnumerable<KanbanColumn> columns) : base(columns)
    {
    }

    public Task<IEnumerable<KanbanColumn>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<KanbanColumn>>(
            SavedItems.Where(column => column.BoardId == boardId).OrderBy(column => column.Order).ToList());
    }

    public Task<bool> OrderExistsAsync(Guid boardId, int order, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.Any(column => column.BoardId == boardId && column.Order == order));
    }
}
