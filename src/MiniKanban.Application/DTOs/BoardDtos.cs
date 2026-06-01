using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateBoardDto
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
}

public class UpdateBoardDto
{
    [MaxLength(255)] public string? Name { get; set; }

    public string? Description { get; set; }
}

public class BoardResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}