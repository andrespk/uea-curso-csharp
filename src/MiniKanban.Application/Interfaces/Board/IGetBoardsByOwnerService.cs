using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Board;

public interface IGetBoardsByOwnerService
{
    Task<IEnumerable<BoardResponseDto>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}