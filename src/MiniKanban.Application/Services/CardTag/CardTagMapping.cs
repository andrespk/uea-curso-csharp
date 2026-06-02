using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.CardTag;

internal static class CardTagMapping
{
    public static CardTagResponseDto ToResponse(Domain.Entities.CardTag cardTag)
    {
        return cardTag.Adapt<CardTagResponseDto>();
    }

    public static Domain.Entities.CardTag ToEntity(CreateCardTagDto dto)
    {
        return dto.Adapt<Domain.Entities.CardTag>();
    }
}