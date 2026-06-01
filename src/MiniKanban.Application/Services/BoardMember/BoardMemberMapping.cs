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
}
