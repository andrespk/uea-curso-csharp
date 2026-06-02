using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Board;

public interface IGetBoardsByUserService
{
    Task<IEnumerable<BoardResponseDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}