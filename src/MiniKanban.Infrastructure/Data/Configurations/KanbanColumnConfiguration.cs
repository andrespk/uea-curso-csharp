using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class KanbanColumnConfiguration : IEntityTypeConfiguration<KanbanColumn>
{
    public void Configure(EntityTypeBuilder<KanbanColumn> builder)
    {
        builder.ToTable("kanban_columns");

        builder.HasKey(column => column.Id);

        builder.Property(column => column.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(column => column.Board)
            .WithMany(board => board.Columns)
            .HasForeignKey(column => column.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}