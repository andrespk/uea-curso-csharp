using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Entities.Abstractions;

namespace MiniKanban.Domain.Entities;

public class Tag : BaseEntity
{
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Color { get; set; }

    public ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
