using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Abstractions;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository, ScopedInjection
{
    public UserRepository(MiniKanbanDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Context.Set<User>().AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await Context.Set<User>().AnyAsync(u => u.Username == username);
    }
}
