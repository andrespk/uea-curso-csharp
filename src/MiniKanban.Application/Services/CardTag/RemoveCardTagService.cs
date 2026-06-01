using MiniKanban.Application.Interfaces.CardTag;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.CardTag;

public class RemoveCardTagService : IRemoveCardTagService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly ICardTagRepository _cardTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCardTagService(
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

    public async Task RemoveAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(cardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        if (await _tagRepository.GetByIdAsync(tagId, cancellationToken) == null)
            throw new BusinessException("Tag not found.");

        if (!await _cardTagRepository.ExistsAsync(cardId, tagId, cancellationToken))
            throw new BusinessException("This tag is not associated with this card.");

        await _cardTagRepository.DeleteByCardAndTagAsync(cardId, tagId, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}