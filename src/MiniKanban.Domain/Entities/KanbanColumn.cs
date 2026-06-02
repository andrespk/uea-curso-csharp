using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Abstractions;

namespace MiniKanban.Domain.Entities;

public class KanbanColumn : BaseEntity
{
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public int Order { get; set; }
    public int? WipLimit { get; set; }

    public ICollection<Card> Cards { get; set; } = new List<Card>();
}