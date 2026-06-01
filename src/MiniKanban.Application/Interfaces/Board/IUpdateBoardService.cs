using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Board;

public interface IUpdateBoardService
{
    Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardDto request, CancellationToken cancellationToken = default);
}