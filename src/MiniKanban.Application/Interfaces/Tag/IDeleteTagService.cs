using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IDeleteTagService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

