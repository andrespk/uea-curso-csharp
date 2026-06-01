using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IDeleteCardService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

