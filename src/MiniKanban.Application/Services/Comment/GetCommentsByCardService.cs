using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetCommentsByCardService : IGetCommentsByCardService, ScopedInjection
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICardRepository _cardRepository;

    public GetCommentsByCardService(
        ICommentRepository commentRepository,
        ICardRepository cardRepository)
    {
        _commentRepository = commentRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(cardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        var comments = await _commentRepository.GetByCardIdAsync(cardId, cancellationToken);
        return comments.Select(c => CommentMapping.ToResponse(c));
    }
}


