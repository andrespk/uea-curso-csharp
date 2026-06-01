using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.CardTag;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.CardTag;

public class GetCardTagsService : IGetCardTagsService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly ICardTagRepository _cardTagRepository;

    public GetCardTagsService(
        ICardTagRepository cardTagRepository,
        ICardRepository cardRepository)
    {
        _cardTagRepository = cardTagRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<CardTagResponseDto>> GetByCardIdAsync(Guid cardId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(cardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        var cardTags = await _cardTagRepository.GetByCardIdAsync(cardId, cancellationToken);
        return cardTags.Select(ct => CardTagMapping.ToResponse(ct));
    }
}