namespace MiniKanban.Domain.Entities;

public class CardTag
{
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
