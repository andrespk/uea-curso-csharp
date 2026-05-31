namespace MiniKanban.Domain.Entities.Abstractions;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeleteddAt { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? UpdatedByUserId { get; set; }
    public int? DeleteddByusrId { get; set; }
    public bool IsActive { get; set; } = true;
}

