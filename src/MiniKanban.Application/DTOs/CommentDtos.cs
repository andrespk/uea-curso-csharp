using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateCommentDto
{
    public Guid CardId { get; set; }
    public Guid UserId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}

public class UpdateCommentDto
{
    [Required]
    public string Content { get; set; } = string.Empty;
}

public class CommentResponseDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
