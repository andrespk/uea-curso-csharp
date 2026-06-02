using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.KanbanColumn;

public interface IUpdateKanbanColumnService
{
    Task<KanbanColumnResponseDto> UpdateAsync(Guid id, UpdateKanbanColumnDto request,
        CancellationToken cancellationToken = default);
}