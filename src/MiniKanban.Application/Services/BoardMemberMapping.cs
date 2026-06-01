using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class BoardMemberMapping
{
    public static BoardMemberResponseDto ToResponse(BoardMember member)
    {
        return new BoardMemberResponseDto
        {
            Id = member.Id,
            BoardId = member.BoardId,
            UserId = member.UserId,
            Role = member.Role,
            JoinedAt = member.JoinedAt
        };
    }
}
