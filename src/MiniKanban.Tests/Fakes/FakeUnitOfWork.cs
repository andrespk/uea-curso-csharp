using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public int CommitCount { get; private set; }

    public Task<int> CommitAsync()
    {
        CommitCount++;
        return Task.FromResult(CommitCount);
    }

    public void Dispose()
    {
    }
}
