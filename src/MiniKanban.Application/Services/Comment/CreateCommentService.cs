using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Comment;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Comment;

public class CreateCommentService : ICreateCommentService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateCommentService(
        ICommentRepository commentRepository,
        ICardRepository cardRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _cardRepository = cardRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CommentResponseDto> CreateAsync(CreateCommentDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _cardRepository.GetByIdAsync(request.CardId, cancellationToken) == null)
            throw new BusinessException("Card not found.");

        if (await _userRepository.GetByIdAsync(request.UserId, cancellationToken) == null)
            throw new BusinessException("User not found.");

        var comment = CommentMapping.ToEntity(request);
        await _commentRepository.AddAsync(comment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CommentMapping.ToResponse(comment);
    }
}