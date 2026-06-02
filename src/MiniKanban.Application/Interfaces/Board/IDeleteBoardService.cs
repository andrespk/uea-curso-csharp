namespace MiniKanban.Application.Interfaces.Board;

public interface IDeleteBoardService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}