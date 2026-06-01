using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class CardTagMapping
{
    public static CardTagResponseDto ToResponse(CardTag cardTag)
    {
        return cardTag.Adapt<CardTagResponseDto>();
    }

    public static CardTag ToEntity(CreateCardTagDto dto)
    {
        return dto.Adapt<CardTag>();
    }
}

