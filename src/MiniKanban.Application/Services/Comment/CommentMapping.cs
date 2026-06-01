using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class CommentMapping
{
    public static CommentResponseDto ToResponse(Comment comment)
    {
        return comment.Adapt<CommentResponseDto>();
    }

    public static Comment ToEntity(CreateCommentDto dto)
    {
        return dto.Adapt<Comment>();
    }

    public static Comment ToEntity(UpdateCommentDto dto, Comment entity)
    {
        return dto.Adapt(entity);
    }
}

