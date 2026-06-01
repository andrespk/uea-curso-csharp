using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.KanbanColumn;

public interface IGetKanbanColumnsByBoardService
{
    Task<IEnumerable<KanbanColumnResponseDto>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default);
}