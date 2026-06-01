using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ICreateCommentService
{
    Task<CommentResponseDto> CreateAsync(CreateCommentDto request, CancellationToken cancellationToken = default);
}

