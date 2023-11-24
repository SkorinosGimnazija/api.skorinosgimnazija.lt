namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.CMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasOne(x => x.LinkedPost).WithOne().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MenuLocation).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ParentMenu).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.Slug, x.LanguageId }).IsUnique();
        builder.HasIndex(x => x.IsPublished);
        builder.HasIndex(x => x.Order);

        builder.Property(x => x.Title).HasMaxLength(256);
        builder.Property(x => x.Slug).HasMaxLength(100);
        builder.Property(x => x.Path).HasMaxLength(300);
        builder.Property(x => x.Url).HasMaxLength(300);
    }
}