using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.Repositories;

namespace MiniKanban.Tests.Fakes;

public class FakeBoardMemberRepository : FakeRepository<BoardMember>, IBoardMemberRepository
{
    public FakeBoardMemberRepository()
    {
    }

    public FakeBoardMemberRepository(IEnumerable<BoardMember> members) : base(members)
    {
    }

    public Task<IEnumerable<BoardMember>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        var members = boardId == Guid.Empty
            ? SavedItems
            : SavedItems.Where(member => member.BoardId == boardId).ToList();

        return Task.FromResult<IEnumerable<BoardMember>>(members);
    }

    public Task<BoardMember?> GetByBoardAndUserAsync(Guid boardId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            SavedItems.FirstOrDefault(member => member.BoardId == boardId && member.UserId == userId));
    }

    public Task<bool> ExistsAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.Any(member => member.BoardId == boardId && member.UserId == userId));
    }
}