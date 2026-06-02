using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.KanbanColumn;

internal static class KanbanColumnMapping
{
    public static KanbanColumnResponseDto ToResponse(Domain.Entities.KanbanColumn column)
    {
        return column.Adapt<KanbanColumnResponseDto>();
    }

    public static Domain.Entities.KanbanColumn ToEntity(CreateKanbanColumnDto dto)
    {
        return dto.Adapt<Domain.Entities.KanbanColumn>();
    }

    public static Domain.Entities.KanbanColumn ToEntity(UpdateKanbanColumnDto dto, Domain.Entities.KanbanColumn entity)
    {
        return dto.Adapt(entity);
    }
}