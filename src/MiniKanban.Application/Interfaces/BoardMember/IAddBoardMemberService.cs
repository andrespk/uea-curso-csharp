using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.BoardMember;

public interface IAddBoardMemberService
{
    Task<BoardMemberResponseDto> AddAsync(CreateBoardMemberDto request, CancellationToken cancellationToken = default);
}