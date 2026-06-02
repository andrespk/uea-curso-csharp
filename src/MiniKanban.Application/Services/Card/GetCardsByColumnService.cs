using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Card;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Card;

public class GetCardsByColumnService : IGetCardsByColumnService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;

    public GetCardsByColumnService(ICardRepository cardRepository, IKanbanColumnRepository kanbanColumnRepository)
    {
        _cardRepository = cardRepository;
        _kanbanColumnRepository = kanbanColumnRepository;
    }

    public async Task<IEnumerable<CardResponseDto>> GetByColumnIdAsync(Guid columnId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _kanbanColumnRepository.GetByIdAsync(columnId, cancellationToken) == null)
            throw new BusinessException("Kanban column not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var cards = await _cardRepository.GetByColumnIdAsync(columnId, cancellationToken);
        return cards.Select(CardMapping.ToResponse);
    }
}