namespace MiniKanban.Application.Interfaces.Card;

public interface IDeleteCardService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}