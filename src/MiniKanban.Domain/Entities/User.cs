using System.ComponentModel.DataAnnotations;
using MiniKanban.Domain.Abstractions;

namespace MiniKanban.Domain.Entities;

public class User : BaseEntity
{
    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string PasswordHash { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string Role { get; set; } = string.Empty;

    public ICollection<Board> CreatedBoards { get; set; } = new List<Board>();
    public ICollection<BoardMember> BoardMemberships { get; set; } = new List<BoardMember>();
    public ICollection<Card> CreatedCards { get; set; } = new List<Card>();
    public ICollection<Card> AssignedCards { get; set; } = new List<Card>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}