using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Card;

public interface IGetCardsByColumnService
{
    Task<IEnumerable<CardResponseDto>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
}