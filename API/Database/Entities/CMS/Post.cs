namespace API.Database.Entities.CMS;

using API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Post
{
    public int Id { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPublished { get; set; }

    public bool ShowInFeed { get; set; }

    public DateTime PublishedAt { get; set; }

    public string LanguageId { get; set; } = null!;

    public Language Language { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string NormalizedTitle { get; private init; } = null!;

    public string Slug { get; set; } = null!;

    public string? IntroText { get; set; }

    public string? Text { get; set; }

    public string? FeaturedImage { get; set; }

    public string? Meta { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public List<string>? Files { get; set; }

    public List<string>? Images { get; set; }
}

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public const int SlugLength = 256;
    public const int TitleLength = 256;
    public const int MetaLength = 256;

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasIndex(x => new
            { x.LanguageId, x.IsPublished, x.ShowInFeed, x.IsFeatured, x.PublishedAt });
        builder.HasIndex(x => new { x.IsFeatured, x.PublishedAt });

        builder.HasIndex(x => x.NormalizedTitle).GinTrigram();
        builder.GenerateNormalized(s => s.Title, t => t.NormalizedTitle);

        builder.Property(x => x.Slug).HasMaxLength(SlugLength);
        builder.Property(x => x.Title).HasMaxLength(TitleLength);
        builder.Property(x => x.Meta).HasMaxLength(MetaLength);

        builder.HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}