namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasOne(x => x.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.IsPublished);
        builder.HasIndex(x => x.IsFeatured);
        builder.HasIndex(x => x.PublishedAt);

        builder.Property(x => x.Slug).HasMaxLength(256);
        builder.Property(x => x.Title).HasMaxLength(256);
        builder.Property(x => x.Meta).HasMaxLength(256);
        builder.Property(x => x.FeaturedImage).HasMaxLength(256);
    }
}