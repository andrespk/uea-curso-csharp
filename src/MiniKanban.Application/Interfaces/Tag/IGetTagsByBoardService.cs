using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Tag;

public interface IGetTagsByBoardService
{
    Task<IEnumerable<TagResponseDto>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
}