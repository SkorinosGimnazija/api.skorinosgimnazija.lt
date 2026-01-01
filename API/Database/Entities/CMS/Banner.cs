namespace API.Database.Entities.CMS;

using API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Banner
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string NormalizedTitle { get; private init; } = null!;

    public string Url { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }

    public bool IsPublished { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int Order { get; set; }

    public string LanguageId { get; set; } = null!;

    public Language Language { get; set; } = null!;
}

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public const int TitleLength = 256;
    public const int UrlLength = 256;
    public const int ImageUrlLength = 256;

    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasIndex(x => new { x.LanguageId, x.IsPublished, x.Order, x.Id });
        builder.HasIndex(x => x.NormalizedTitle).GinTrigram();
        builder.GenerateNormalized(s => s.Title, t => t.NormalizedTitle);

        builder.Property(x => x.Title).HasMaxLength(TitleLength);
        builder.Property(x => x.Url).HasMaxLength(UrlLength);
        builder.Property(x => x.ImageUrl).HasMaxLength(ImageUrlLength);

        builder.HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}