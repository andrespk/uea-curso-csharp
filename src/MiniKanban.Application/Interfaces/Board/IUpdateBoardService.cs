using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateBoardService
{
    Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardDto request);
}
