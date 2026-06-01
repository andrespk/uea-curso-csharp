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

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();

        modelBuilder.Entity<Board>()
            .HasOne(board => board.Owner)
            .WithMany(user => user.CreatedBoards)
            .HasForeignKey(board => board.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BoardMember>()
            .HasIndex(member => new { member.BoardId, member.UserId })
            .IsUnique();

        modelBuilder.Entity<BoardMember>()
            .HasOne(member => member.Board)
            .WithMany(board => board.Members)
            .HasForeignKey(member => member.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardMember>()
            .HasOne(member => member.User)
            .WithMany(user => user.BoardMemberships)
            .HasForeignKey(member => member.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KanbanColumn>()
            .HasOne(column => column.Board)
            .WithMany(board => board.Columns)
            .HasForeignKey(column => column.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Card>()
            .HasOne(card => card.Column)
            .WithMany(column => column.Cards)
            .HasForeignKey(card => card.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Card>()
            .HasOne(card => card.CreatedByUser)
            .WithMany(user => user.CreatedCards)
            .HasForeignKey(card => card.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(card => card.AssignedToUser)
            .WithMany(user => user.AssignedCards)
            .HasForeignKey(card => card.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Card)
            .WithMany(card => card.Comments)
            .HasForeignKey(comment => comment.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.User)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tag>()
            .HasIndex(tag => new { tag.BoardId, tag.Name })
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasOne(tag => tag.Board)
            .WithMany(board => board.Tags)
            .HasForeignKey(tag => tag.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardTag>()
            .HasKey(cardTag => new { cardTag.CardId, cardTag.TagId });

        modelBuilder.Entity<CardTag>()
            .HasOne(cardTag => cardTag.Card)
            .WithMany(card => card.CardTags)
            .HasForeignKey(cardTag => cardTag.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardTag>()
            .HasOne(cardTag => cardTag.Tag)
            .WithMany(tag => tag.CardTags)
            .HasForeignKey(cardTag => cardTag.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


