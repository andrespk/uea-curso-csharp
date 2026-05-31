namespace MiniKanban.Infrastructure.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}
