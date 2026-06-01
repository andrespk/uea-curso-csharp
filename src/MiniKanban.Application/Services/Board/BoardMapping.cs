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

    public static Board ToEntity(CreateBoardDto dto)
    {
        return dto.Adapt<Board>();
    }

    public static Board ToEntity(UpdateBoardDto dto, Board entity)
    {
        return dto.Adapt(entity);
    }
}
