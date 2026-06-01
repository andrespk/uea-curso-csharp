using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IDeleteKanbanColumnService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
