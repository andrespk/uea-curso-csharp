namespace MiniKanban.Application.Interfaces;

public interface IDeleteBoardService
{
    Task DeleteAsync(Guid id);
}
