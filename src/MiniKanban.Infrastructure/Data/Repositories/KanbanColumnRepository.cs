using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class KanbanColumnRepository : Repository<KanbanColumn>, IKanbanColumnRepository, ScopedInjection
{
    public KanbanColumnRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<KanbanColumn>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default)
    {
        return await Context.KanbanColumns
            .Where(column => column.BoardId == boardId)
            .OrderBy(column => column.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> OrderExistsAsync(Guid boardId, int order, CancellationToken cancellationToken = default)
    {
        return await Context.KanbanColumns
            .AnyAsync(column => column.BoardId == boardId && column.Order == order, cancellationToken);
    }
}