using Microsoft.EntityFrameworkCore;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Context;

public class MiniKanbanDbContext : DbContext
{
    public MiniKanbanDbContext(DbContextOptions<MiniKanbanDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardMember> BoardMembers { get; set; }
    public DbSet<KanbanColumn> KanbanColumns { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CardTag> CardTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniKanbanDbContext).Assembly);
    }
}


