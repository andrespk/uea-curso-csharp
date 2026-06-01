using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class BoardRepository : Repository<Board>, IBoardRepository, ScopedInjection
{
    public BoardRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Board>> GetByOwnerIdAsync(Guid ownerId)
    {
        return await Context.Boards
            .Where(board => board.OwnerId == ownerId)
            .OrderBy(board => board.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Board>> GetByMemberUserIdAsync(Guid userId)
    {
        return await Context.BoardMembers
            .Where(member => member.UserId == userId)
            .Select(member => member.Board)
            .OrderBy(board => board.Name)
            .ToListAsync();
    }
}
