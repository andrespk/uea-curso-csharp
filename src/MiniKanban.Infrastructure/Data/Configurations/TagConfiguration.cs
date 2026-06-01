using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(tag => tag.Id);

        builder.Property(tag => tag.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(tag => tag.Color)
            .HasMaxLength(50);

        builder.HasIndex(tag => new { tag.BoardId, tag.Name })
            .IsUnique();

        builder.HasOne(tag => tag.Board)
            .WithMany(board => board.Tags)
            .HasForeignKey(tag => tag.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}