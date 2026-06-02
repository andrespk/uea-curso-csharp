using MiniKanban.Domain.Enums;

namespace MiniKanban.Application.DTOs;

public class CreateBoardMemberDto
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public BoardRole Role { get; set; } = BoardRole.Member;
}

public class UpdateBoardMemberDto
{
    public BoardRole Role { get; set; }
}

public class BoardMemberResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public BoardRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}