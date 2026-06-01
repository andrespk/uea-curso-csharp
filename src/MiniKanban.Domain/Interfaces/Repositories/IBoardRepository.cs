using System.Threading;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<IEnumerable<Board>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Board>> GetByMemberUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
