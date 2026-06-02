using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.Comment;

internal static class CommentMapping
{
    public static CommentResponseDto ToResponse(Domain.Entities.Comment comment)
    {
        return comment.Adapt<CommentResponseDto>();
    }

    public static Domain.Entities.Comment ToEntity(CreateCommentDto dto)
    {
        return dto.Adapt<Domain.Entities.Comment>();
    }

    public static Domain.Entities.Comment ToEntity(UpdateCommentDto dto, Domain.Entities.Comment entity)
    {
        return dto.Adapt(entity);
    }
}