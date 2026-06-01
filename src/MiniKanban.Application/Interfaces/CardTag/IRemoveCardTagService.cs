using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IRemoveCardTagService
{
    Task RemoveAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default);
}

