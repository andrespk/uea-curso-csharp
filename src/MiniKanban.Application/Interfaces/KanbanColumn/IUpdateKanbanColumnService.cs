using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateKanbanColumnService
{
    Task<KanbanColumnResponseDto> UpdateAsync(Guid id, UpdateKanbanColumnDto request, CancellationToken cancellationToken = default);
}
