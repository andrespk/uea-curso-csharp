using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Entities.Abstractions;

namespace MiniKanban.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public string Content { get; set; } = string.Empty;
}
