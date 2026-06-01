using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork, ScopedInjection
{
    private readonly MiniKanbanDbContext _context;

    public UnitOfWork(MiniKanbanDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
