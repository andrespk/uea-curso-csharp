using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Board;

public interface IGetBoardByIdService
{
    Task<BoardResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}