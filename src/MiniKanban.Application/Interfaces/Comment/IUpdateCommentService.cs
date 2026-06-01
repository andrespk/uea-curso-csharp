using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateCommentService
{
    Task<CommentResponseDto> UpdateAsync(Guid id, UpdateCommentDto request, CancellationToken cancellationToken = default);
}

