using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository, ScopedInjection
{
    public TagRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tag>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await Context.Tags
            .Where(t => t.BoardId == boardId)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> NameExistsAsync(Guid boardId, string name, CancellationToken cancellationToken = default)
    {
        return await Context.Tags
            .AnyAsync(t => t.BoardId == boardId && t.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}