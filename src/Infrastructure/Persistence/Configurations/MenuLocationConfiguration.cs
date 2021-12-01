namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MenuLocationConfiguration : IEntityTypeConfiguration<MenuLocation>
{
    public void Configure(EntityTypeBuilder<MenuLocation> builder)
    {
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Slug).HasMaxLength(100);
    }
}