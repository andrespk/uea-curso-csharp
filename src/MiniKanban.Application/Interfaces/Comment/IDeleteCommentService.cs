namespace MiniKanban.Application.Interfaces.Comment;

public interface IDeleteCommentService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}