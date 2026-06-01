using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<IEnumerable<Board>> GetByOwnerIdAsync(Guid ownerId);
    Task<IEnumerable<Board>> GetByMemberUserIdAsync(Guid userId);
}
