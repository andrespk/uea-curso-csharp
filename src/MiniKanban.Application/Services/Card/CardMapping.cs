using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.Card;

internal static class CardMapping
{
    public static CardResponseDto ToResponse(Domain.Entities.Card card)
    {
        return card.Adapt<CardResponseDto>();
    }

    public static Domain.Entities.Card ToEntity(CreateCardDto dto)
    {
        return dto.Adapt<Domain.Entities.Card>();
    }

    public static Domain.Entities.Card ToEntity(UpdateCardDto dto, Domain.Entities.Card entity)
    {
        return dto.Adapt(entity);
    }
}