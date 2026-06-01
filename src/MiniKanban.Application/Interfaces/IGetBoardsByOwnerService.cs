using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetBoardsByOwnerService
{
    Task<IEnumerable<BoardResponseDto>> GetByOwnerIdAsync(Guid ownerId);
}
