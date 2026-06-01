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
}
