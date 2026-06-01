using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces.Repositories;

public interface ICardRepository : IRepository<Card>
{
    Task<IEnumerable<Card>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Card>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    Task<int> CountByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
}