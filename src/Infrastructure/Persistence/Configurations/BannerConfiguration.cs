namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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