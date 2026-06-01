using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Tests.Fakes;

public class FakeBoardRepository : FakeRepository<Board>, IBoardRepository
{
    private readonly IBoardMemberRepository? _boardMemberRepository;

    public FakeBoardRepository()
    {
    }

    public FakeBoardRepository(IEnumerable<Board> boards) : base(boards)
    {
    }

    public FakeBoardRepository(IEnumerable<Board> boards, IBoardMemberRepository boardMemberRepository) : base(boards)
    {
        _boardMemberRepository = boardMemberRepository;
    }

    public Task<IEnumerable<Board>> GetByOwnerIdAsync(Guid ownerId)
    {
        return Task.FromResult<IEnumerable<Board>>(SavedItems.Where(board => board.OwnerId == ownerId).ToList());
    }

    public async Task<IEnumerable<Board>> GetByMemberUserIdAsync(Guid userId)
    {
        if (_boardMemberRepository == null)
            return SavedItems.Where(board => board.OwnerId == userId).ToList();

        var memberships = await _boardMemberRepository.GetByBoardIdAsync(Guid.Empty);
        var boardIds = memberships
            .Where(member => member.UserId == userId)
            .Select(member => member.BoardId)
            .ToHashSet();

        return SavedItems.Where(board => boardIds.Contains(board.Id)).ToList();
    }
}
