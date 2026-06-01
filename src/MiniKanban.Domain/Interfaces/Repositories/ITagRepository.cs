using System.Threading;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(Guid boardId, string name, CancellationToken cancellationToken = default);
}

