using System.Threading;

namespace MiniKanban.Application.Interfaces;

public interface IRemoveBoardMemberService
{
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
