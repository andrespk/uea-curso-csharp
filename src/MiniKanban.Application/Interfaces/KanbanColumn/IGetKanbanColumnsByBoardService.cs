using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetKanbanColumnsByBoardService
{
    Task<IEnumerable<KanbanColumnResponseDto>> GetByBoardIdAsync(Guid boardId);
}
