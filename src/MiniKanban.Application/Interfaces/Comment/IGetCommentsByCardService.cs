using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Comment;

public interface IGetCommentsByCardService
{
    Task<IEnumerable<CommentResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}