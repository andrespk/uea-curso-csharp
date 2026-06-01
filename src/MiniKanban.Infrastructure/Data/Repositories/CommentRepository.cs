using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository, ScopedInjection
{
    public CommentRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
    {
        return await Context.Comments
            .Include(c => c.User)
            .Where(c => c.CardId == cardId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

