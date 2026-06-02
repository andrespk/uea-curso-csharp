using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateCardDto
{
    public Guid ColumnId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    [Required] [MaxLength(255)] public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateCardDto
{
    public Guid? ColumnId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    [MaxLength(255)] public string? Title { get; set; }

    public string? Description { get; set; }
    public int? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class CardResponseDto
{
    public Guid Id { get; set; }
    public Guid ColumnId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
}