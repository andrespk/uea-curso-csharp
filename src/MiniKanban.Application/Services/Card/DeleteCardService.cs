using MiniKanban.Application.Interfaces.Card;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Card;

public class DeleteCardService : IDeleteCardService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCardService(
        ICardRepository cardRepository,
        IUnitOfWork unitOfWork)
    {
        _cardRepository = cardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var card = await _cardRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new BusinessException("Card not found.");

        await _cardRepository.DeleteAsync(card, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}