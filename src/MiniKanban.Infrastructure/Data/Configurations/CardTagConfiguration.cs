using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class CardTagConfiguration : IEntityTypeConfiguration<CardTag>
{
    public void Configure(EntityTypeBuilder<CardTag> builder)
    {
        builder.ToTable("card_tags");

        builder.HasKey(cardTag => new { cardTag.CardId, cardTag.TagId });

        builder.HasOne(cardTag => cardTag.Card)
            .WithMany(card => card.CardTags)
            .HasForeignKey(cardTag => cardTag.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cardTag => cardTag.Tag)
            .WithMany(tag => tag.CardTags)
            .HasForeignKey(cardTag => cardTag.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}