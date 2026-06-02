using MiniKanban.Domain.Abstractions;
using MiniKanban.Domain.Enums;

namespace MiniKanban.Domain.Entities;

public class BoardMember : BaseEntity
{
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public BoardRole Role { get; set; } = BoardRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}