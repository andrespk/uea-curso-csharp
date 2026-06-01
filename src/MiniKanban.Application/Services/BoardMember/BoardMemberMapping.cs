using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class BoardMemberMapping
{
    public static BoardMemberResponseDto ToResponse(BoardMember member)
    {
        return member.Adapt<BoardMemberResponseDto>();
    }

    public static BoardMember ToEntity(CreateBoardMemberDto dto)
    {
        return dto.Adapt<BoardMember>();
    }

    public static BoardMember ToEntity(UpdateBoardMemberDto dto, BoardMember entity)
    {
        return dto.Adapt(entity);
    }
}
