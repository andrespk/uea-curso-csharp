using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateCommentService : IUpdateCommentService, ScopedInjection
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentService(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CommentResponseDto> UpdateAsync(Guid id, UpdateCommentDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Comment not found.");

        var updatedComment = CommentMapping.ToEntity(request, comment);
        await _commentRepository.UpdateAsync(updatedComment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CommentMapping.ToResponse(updatedComment);
    }
}

