using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Comment;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Comment;

public class GetCommentsByCardService : IGetCommentsByCardService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly ICommentRepository _commentRepository;

    public GetCommentsByCardService(
        ICommentRepository commentRepository,
        ICardRepository cardRepository)
    {
        _commentRepository = commentRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetByCardIdAsync(Guid cardId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(cardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        var comments = await _commentRepository.GetByCardIdAsync(cardId, cancellationToken);
        return comments.Select(c => CommentMapping.ToResponse(c));
    }
}