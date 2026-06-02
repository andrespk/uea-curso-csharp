using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository, ScopedInjection
{
    public UserRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().AnyAsync(u => u.Username == username, cancellationToken);
    }
}