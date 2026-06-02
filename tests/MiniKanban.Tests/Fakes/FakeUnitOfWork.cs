using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public int CommitCount { get; private set; }

    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        CommitCount++;
        return Task.FromResult(CommitCount);
    }

    public void Dispose()
    {
    }
}