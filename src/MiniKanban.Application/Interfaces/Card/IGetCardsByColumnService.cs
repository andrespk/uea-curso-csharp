using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetCardsByColumnService
{
    Task<IEnumerable<CardResponseDto>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
}
