namespace MiniKanban.Application.Interfaces.KanbanColumn;

public interface IDeleteKanbanColumnService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}