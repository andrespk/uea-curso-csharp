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

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
