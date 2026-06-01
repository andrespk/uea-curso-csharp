using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateTagDto
{
    public Guid BoardId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Color { get; set; }
}

public class UpdateTagDto
{
    [MaxLength(255)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }
}

public class TagResponseDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}
