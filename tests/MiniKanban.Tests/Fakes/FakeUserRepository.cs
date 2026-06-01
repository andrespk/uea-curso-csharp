using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.Repositories;

namespace MiniKanban.Tests.Fakes;

public class FakeUserRepository : FakeRepository<User>, IUserRepository
{
    public FakeUserRepository()
    {
    }

    public FakeUserRepository(IEnumerable<User> users) : base(users)
    {
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.FirstOrDefault(user => user.Email == email));
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.FirstOrDefault(user => user.Username == username));
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.Any(user => user.Email == email));
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SavedItems.Any(user => user.Username == username));
    }
}