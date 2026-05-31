using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Entities.Abstractions;

namespace MiniKanban.Domain.Entities;

public class Card : BaseEntity
{
    public Guid ColumnId { get; set; }
    public KanbanColumn Column { get; set; } = null!;

    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
