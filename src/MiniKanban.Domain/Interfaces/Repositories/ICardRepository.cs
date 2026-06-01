using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface ICardRepository : IRepository<Card>
{
    Task<IEnumerable<Card>> GetByColumnIdAsync(Guid columnId);
    Task<IEnumerable<Card>> GetByBoardIdAsync(Guid boardId);
    Task<int> CountByColumnIdAsync(Guid columnId);
}
