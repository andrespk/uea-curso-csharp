using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class CardTagRepository : Repository<CardTag>, ICardTagRepository, ScopedInjection
{
    public CardTagRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CardTag>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
    {
        return await Context.CardTags
            .Include(ct => ct.Tag)
            .Where(ct => ct.CardId == cardId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default)
    {
        return await Context.CardTags
            .AnyAsync(ct => ct.CardId == cardId && ct.TagId == tagId, cancellationToken);
    }

    public async Task DeleteByCardAndTagAsync(Guid cardId, Guid tagId, CancellationToken cancellationToken = default)
    {
        var cardTag = await Context.CardTags
            .FirstOrDefaultAsync(ct => ct.CardId == cardId && ct.TagId == tagId, cancellationToken);

        if (cardTag != null) Context.CardTags.Remove(cardTag);
    }
}