using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.BoardMember;

internal static class BoardMemberMapping
{
    public static BoardMemberResponseDto ToResponse(Domain.Entities.BoardMember member)
    {
        return member.Adapt<BoardMemberResponseDto>();
    }

    public static Domain.Entities.BoardMember ToEntity(CreateBoardMemberDto dto)
    {
        return dto.Adapt<Domain.Entities.BoardMember>();
    }

    public static Domain.Entities.BoardMember ToEntity(UpdateBoardMemberDto dto, Domain.Entities.BoardMember entity)
    {
        return dto.Adapt(entity);
    }
}