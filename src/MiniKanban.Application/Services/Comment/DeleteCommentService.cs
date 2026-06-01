using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class DeleteCommentService : IDeleteCommentService, ScopedInjection
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentService(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Comment not found.");

        await _commentRepository.DeleteAsync(comment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

