using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetBoardMembersService
{
    Task<IEnumerable<BoardMemberResponseDto>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
}
