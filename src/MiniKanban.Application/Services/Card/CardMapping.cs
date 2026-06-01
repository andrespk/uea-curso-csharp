using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class CardMapping
{
    public static CardResponseDto ToResponse(Card card)
    {
        return card.Adapt<CardResponseDto>();
    }

    public static Card ToEntity(CreateCardDto dto)
    {
        return dto.Adapt<Card>();
    }

    public static Card ToEntity(UpdateCardDto dto, Card entity)
    {
        return dto.Adapt(entity);
    }
}
