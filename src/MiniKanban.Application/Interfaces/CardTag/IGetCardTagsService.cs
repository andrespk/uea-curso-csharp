using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetCardTagsService
{
    Task<IEnumerable<CardTagResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}

