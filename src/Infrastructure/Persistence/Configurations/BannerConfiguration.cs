namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Domain.Entities.CMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasOne(x => x.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.IsPublished);
        builder.HasIndex(x => x.Order);

        builder.Property(x => x.Title).HasMaxLength(100);
        builder.Property(x => x.PictureUrl).HasMaxLength(256);
        builder.Property(x => x.Url).HasMaxLength(256);
    }
}