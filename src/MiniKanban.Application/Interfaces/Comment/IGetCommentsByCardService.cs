using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetCommentsByCardService
{
    Task<IEnumerable<CommentResponseDto>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}

