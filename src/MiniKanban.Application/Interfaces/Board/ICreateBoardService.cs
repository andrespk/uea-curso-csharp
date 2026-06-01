using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ICreateBoardService
{
    Task<BoardResponseDto> CreateAsync(CreateBoardDto request);
}
