using System.Threading;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface IKanbanColumnRepository : IRepository<KanbanColumn>
{
    Task<IEnumerable<KanbanColumn>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    Task<bool> OrderExistsAsync(Guid boardId, int order, CancellationToken cancellationToken = default);
}
