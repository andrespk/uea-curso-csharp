using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Board;

public interface ICreateBoardService
{
    Task<BoardResponseDto> CreateAsync(CreateBoardDto request, CancellationToken cancellationToken = default);
}