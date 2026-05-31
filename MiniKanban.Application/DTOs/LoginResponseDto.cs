using System.ComponentModel.DataAnnotations;

namespace MiniKanban.Application.DTOs;

public class LoginResponseDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;
}
