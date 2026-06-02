using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.CardTag;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.CardTag;

public class AddCardTagService : IAddCardTagService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly ICardTagRepository _cardTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCardTagService(
        ICardTagRepository cardTagRepository,
        ICardRepository cardRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _cardTagRepository = cardTagRepository;
        _cardRepository = cardRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CardTagResponseDto> AddAsync(CreateCardTagDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(request.CardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        if (await _tagRepository.GetByIdAsync(request.TagId, cancellationToken) == null)
            throw new BusinessException("Tag not found.");

        if (await _cardTagRepository.ExistsAsync(request.CardId, request.TagId, cancellationToken))
            throw new BusinessException("This tag is already associated with this card.");

        var cardTag = CardTagMapping.ToEntity(request);
        await _cardTagRepository.AddAsync(cardTag, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CardTagMapping.ToResponse(cardTag);
    }
}