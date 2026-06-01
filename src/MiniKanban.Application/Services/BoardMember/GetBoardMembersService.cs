using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.BoardMember;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.BoardMember;

public class GetBoardMembersService : IGetBoardMembersService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IBoardRepository _boardRepository;

    public GetBoardMembersService(IBoardMemberRepository boardMemberRepository, IBoardRepository boardRepository)
    {
        _boardMemberRepository = boardMemberRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<BoardMemberResponseDto>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(boardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var members = await _boardMemberRepository.GetByBoardIdAsync(boardId, cancellationToken);
        return members.Select(BoardMemberMapping.ToResponse);
    }
}