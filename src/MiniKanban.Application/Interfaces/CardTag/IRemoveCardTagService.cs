namespace MiniKanban.Application.Interfaces.CardTag;

public interface IRemoveCardTagService
{
    Task RemoveAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default);
}