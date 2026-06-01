using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface IBoardMemberRepository : IRepository<BoardMember>
{
    Task<IEnumerable<BoardMember>> GetByBoardIdAsync(Guid boardId);
    Task<BoardMember?> GetByBoardAndUserAsync(Guid boardId, Guid userId);
    Task<bool> ExistsAsync(Guid boardId, Guid userId);
}
