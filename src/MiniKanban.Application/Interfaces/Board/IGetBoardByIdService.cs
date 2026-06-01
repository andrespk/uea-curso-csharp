using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetBoardByIdService
{
    Task<BoardResponseDto> GetByIdAsync(Guid id);
}
