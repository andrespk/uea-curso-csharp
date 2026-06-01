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

    public static KanbanColumn ToEntity(CreateKanbanColumnDto dto)
    {
        return dto.Adapt<KanbanColumn>();
    }

    public static KanbanColumn ToEntity(UpdateKanbanColumnDto dto, KanbanColumn entity)
    {
        return dto.Adapt(entity);
    }
}
