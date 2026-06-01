using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.KanbanColumn;

public interface ICreateKanbanColumnService
{
    Task<KanbanColumnResponseDto> CreateAsync(CreateKanbanColumnDto request,
        CancellationToken cancellationToken = default);
}