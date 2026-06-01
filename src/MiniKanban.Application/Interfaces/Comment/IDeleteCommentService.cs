using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IDeleteCommentService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

