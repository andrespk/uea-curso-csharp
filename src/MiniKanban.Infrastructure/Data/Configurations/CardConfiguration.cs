using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");

        builder.HasKey(card => card.Id);

        builder.Property(card => card.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(card => card.Column)
            .WithMany(column => column.Cards)
            .HasForeignKey(card => card.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(card => card.CreatedByUser)
            .WithMany(user => user.CreatedCards)
            .HasForeignKey(card => card.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(card => card.AssignedToUser)
            .WithMany(user => user.AssignedCards)
            .HasForeignKey(card => card.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
