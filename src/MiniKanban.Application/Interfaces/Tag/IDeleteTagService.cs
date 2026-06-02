namespace MiniKanban.Application.Interfaces.Tag;

public interface IDeleteTagService
{
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}