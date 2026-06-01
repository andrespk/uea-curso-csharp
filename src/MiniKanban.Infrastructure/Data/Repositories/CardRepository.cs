using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class CardRepository : Repository<Card>, ICardRepository, ScopedInjection
{
    public CardRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Card>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default)
    {
        return await Context.Cards
            .Where(card => card.ColumnId == columnId)
            .OrderByDescending(card => card.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Card>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await Context.Cards
            .Include(card => card.Column)
            .Where(card => card.Column.BoardId == boardId)
            .OrderByDescending(card => card.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default)
    {
        return await Context.Cards.CountAsync(card => card.ColumnId == columnId, cancellationToken);
    }
}
