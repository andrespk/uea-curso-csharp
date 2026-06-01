using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.Board;

internal static class BoardMapping
{
    public static BoardResponseDto ToResponse(Domain.Entities.Board board)
    {
        return board.Adapt<BoardResponseDto>();
    }

    public static Domain.Entities.Board ToEntity(CreateBoardDto dto)
    {
        return dto.Adapt<Domain.Entities.Board>();
    }

    public static Domain.Entities.Board ToEntity(UpdateBoardDto dto, Domain.Entities.Board entity)
    {
        return dto.Adapt(entity);
    }
}