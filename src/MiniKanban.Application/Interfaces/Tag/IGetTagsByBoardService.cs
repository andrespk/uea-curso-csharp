using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetTagsByBoardService
{
    Task<IEnumerable<TagResponseDto>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
}

