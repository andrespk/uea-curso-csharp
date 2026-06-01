using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Comment;

public interface ICreateCommentService
{
    Task<CommentResponseDto> CreateAsync(CreateCommentDto request, CancellationToken cancellationToken = default);
}