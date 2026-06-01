using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasIndex(user => user.Username)
            .IsUnique();
    }
}
