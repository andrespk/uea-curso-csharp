using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Context;

public class MiniKanbanDbContext : DbContext
{
    public MiniKanbanDbContext(DbContextOptions<MiniKanbanDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}



