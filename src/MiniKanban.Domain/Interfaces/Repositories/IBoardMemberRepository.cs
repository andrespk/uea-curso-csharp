using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces.Repositories;

public interface IBoardMemberRepository : IRepository<BoardMember>
{
    Task<IEnumerable<BoardMember>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    Task<BoardMember?> GetByBoardAndUserAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default);
}