using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateBoardMemberService
{
    Task<BoardMemberResponseDto> UpdateAsync(Guid id, UpdateBoardMemberDto request, CancellationToken cancellationToken = default);
}
