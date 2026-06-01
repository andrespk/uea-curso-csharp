using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetCardTagsService : IGetCardTagsService, ScopedInjection
{
    private readonly ICardTagRepository _cardTagRepository;
    private readonly ICardRepository _cardRepository;

    public GetCardTagsService(
        ICardTagRepository cardTagRepository,
        ICardRepository cardRepository)
    {
        _cardTagRepository = cardTagRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<CardTagResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(cardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        var cardTags = await _cardTagRepository.GetByCardIdAsync(cardId, cancellationToken);
        return cardTags.Select(ct => CardTagMapping.ToResponse(ct));
    }
}

