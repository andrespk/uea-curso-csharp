using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class CreateUserDto
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string Password { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string Role { get; set; } = string.Empty;
}

public class UpdateUserDto
{
    [MaxLength(255)] public string? Name { get; set; }

    [MaxLength(255)] public string? Username { get; set; }

    [EmailAddress] [MaxLength(255)] public string? Email { get; set; }

    [MaxLength(255)] public string? Password { get; set; }

    [MaxLength(50)] public string? Role { get; set; }
}

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}