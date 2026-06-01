using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class BoardMapping
{
    public static BoardResponseDto ToResponse(Board board)
    {
        return board.Adapt<BoardResponseDto>();
    }
}
