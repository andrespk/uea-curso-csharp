using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class CardMapping
{
    public static CardResponseDto ToResponse(Card card)
    {
        return new CardResponseDto
        {
            Id = card.Id,
            ColumnId = card.ColumnId,
            CreatedByUserId = card.CreatedByUserId,
            AssignedToUserId = card.AssignedToUserId,
            Title = card.Title,
            Description = card.Description,
            Priority = card.Priority,
            CreatedAt = card.CreatedAt,
            DueDate = card.DueDate
        };
    }
}
