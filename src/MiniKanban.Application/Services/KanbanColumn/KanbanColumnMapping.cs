using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class KanbanColumnMapping
{
    public static KanbanColumnResponseDto ToResponse(KanbanColumn column)
    {
        return new KanbanColumnResponseDto
        {
            Id = column.Id,
            BoardId = column.BoardId,
            Name = column.Name,
            Order = column.Order,
            WipLimit = column.WipLimit,
            CreatedAt = column.CreatedAt
        };
    }
}
