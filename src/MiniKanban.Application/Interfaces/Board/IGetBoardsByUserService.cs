using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetBoardsByUserService
{
    Task<IEnumerable<BoardResponseDto>> GetByUserIdAsync(Guid userId);
}
