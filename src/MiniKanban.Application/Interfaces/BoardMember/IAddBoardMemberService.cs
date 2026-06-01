using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IAddBoardMemberService
{
    Task<BoardMemberResponseDto> AddAsync(CreateBoardMemberDto request);
}
