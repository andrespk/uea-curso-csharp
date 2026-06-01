using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateKanbanColumnDto
{
    public Guid BoardId { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public int Order { get; set; }
    public int? WipLimit { get; set; }
}

public class UpdateKanbanColumnDto
{
    [MaxLength(255)] public string? Name { get; set; }

    public int? Order { get; set; }
    public int? WipLimit { get; set; }
}

public class KanbanColumnResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? WipLimit { get; set; }
    public DateTime CreatedAt { get; set; }
}