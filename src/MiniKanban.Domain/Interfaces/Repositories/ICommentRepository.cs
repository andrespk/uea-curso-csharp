using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
}