using System.Threading;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface ICardTagRepository : IRepository<CardTag>
{
    Task<IEnumerable<CardTag>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default);
    Task DeleteByCardAndTagAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default);
}

