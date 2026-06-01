using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.CardTag;

public interface IGetCardTagsService
{
    Task<IEnumerable<CardTagResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}