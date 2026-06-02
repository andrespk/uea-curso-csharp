using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.BoardMember;

public interface IGetBoardMembersService
{
    Task<IEnumerable<BoardMemberResponseDto>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default);
}