using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class KanbanColumnMapping
{
    public static KanbanColumnResponseDto ToResponse(KanbanColumn column)
    {
        return column.Adapt<KanbanColumnResponseDto>();
    }
}
