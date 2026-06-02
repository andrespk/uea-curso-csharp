using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Abstractions;

namespace MiniKanban.Domain.Entities;

public class Board : BaseEntity
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<KanbanColumn> Columns { get; set; } = new List<KanbanColumn>();
    public ICollection<BoardMember> Members { get; set; } = new List<BoardMember>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}