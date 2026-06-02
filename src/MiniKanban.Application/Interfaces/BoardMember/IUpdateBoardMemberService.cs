using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.BoardMember;

public interface IUpdateBoardMemberService
{
    Task<BoardMemberResponseDto> UpdateAsync(Guid id, UpdateBoardMemberDto request,
        CancellationToken cancellationToken = default);
}