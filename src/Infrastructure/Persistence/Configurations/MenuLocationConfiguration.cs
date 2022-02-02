namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Domain.Entities.CMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MenuLocationConfiguration : IEntityTypeConfiguration<MenuLocation>
{
    public void Configure(EntityTypeBuilder<MenuLocation> builder)
    {
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Slug).HasMaxLength(30);
    }
}