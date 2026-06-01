using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class BoardMemberRepository : Repository<BoardMember>, IBoardMemberRepository, ScopedInjection
{
    public BoardMemberRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BoardMember>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        return await Context.BoardMembers
            .Where(member => member.BoardId == boardId)
            .OrderBy(member => member.JoinedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<BoardMember?> GetByBoardAndUserAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.BoardMembers
            .FirstOrDefaultAsync(member => member.BoardId == boardId && member.UserId == userId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid boardId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.BoardMembers
            .AnyAsync(member => member.BoardId == boardId && member.UserId == userId, cancellationToken);
    }
}
