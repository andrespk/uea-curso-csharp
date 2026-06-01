namespace MiniKanban.Application.Interfaces;

public interface IDeleteKanbanColumnService
{
    Task DeleteAsync(Guid id);
}
