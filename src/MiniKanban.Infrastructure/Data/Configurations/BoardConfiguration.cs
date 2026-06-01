using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("boards");

        builder.HasKey(board => board.Id);

        builder.Property(board => board.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(board => board.Owner)
            .WithMany(user => user.CreatedBoards)
            .HasForeignKey(board => board.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
