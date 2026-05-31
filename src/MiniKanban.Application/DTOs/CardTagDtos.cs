namespace MiniKanban.Application.DTOs;

public class CreateCardTagDto
{
    public Guid CardId { get; set; }
    public Guid TagId { get; set; }
}

public class CardTagResponseDto
{
    public Guid CardId { get; set; }
    public Guid TagId { get; set; }
}
