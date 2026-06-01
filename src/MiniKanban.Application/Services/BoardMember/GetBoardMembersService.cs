using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetBoardMembersService : IGetBoardMembersService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IBoardRepository _boardRepository;

    public GetBoardMembersService(IBoardMemberRepository boardMemberRepository, IBoardRepository boardRepository)
    {
        _boardMemberRepository = boardMemberRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<BoardMemberResponseDto>> GetByBoardIdAsync(Guid boardId)
    {
        if (await _boardRepository.GetByIdAsync(boardId) == null)
            throw new BusinessException("Board not found.");

        var members = await _boardMemberRepository.GetByBoardIdAsync(boardId);
        return members.Select(BoardMemberMapping.ToResponse);
    }
}
