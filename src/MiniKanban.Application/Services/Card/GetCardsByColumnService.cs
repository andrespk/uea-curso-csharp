using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetCardsByColumnService : IGetCardsByColumnService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;

    public GetCardsByColumnService(ICardRepository cardRepository, IKanbanColumnRepository kanbanColumnRepository)
    {
        _cardRepository = cardRepository;
        _kanbanColumnRepository = kanbanColumnRepository;
    }

    public async Task<IEnumerable<CardResponseDto>> GetByColumnIdAsync(Guid columnId)
    {
        if (await _kanbanColumnRepository.GetByIdAsync(columnId) == null)
            throw new BusinessException("Kanban column not found.");

        var cards = await _cardRepository.GetByColumnIdAsync(columnId);
        return cards.Select(CardMapping.ToResponse);
    }
}
