namespace API.Database.Entities.CMS;

using API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Menu
{
    public int Id { get; set; }

    public int Order { get; set; }

    public bool IsPublished { get; set; }

    public bool IsHidden { get; set; }

    public string Title { get; set; } = null!;

    public string NormalizedTitle { get; private init; } = null!;

    public string? Url { get; set; }

    public bool IsExternal { get; set; }

    public int? PostId { get; set; }

    public Post? Post { get; set; }

    public string LanguageId { get; set; } = null!;

    public Language Language { get; set; } = null!;

    public int? ParentMenuId { get; set; }

    public Menu? ParentMenu { get; set; }
}

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public const int TitleLength = 256;
    public const int UrlLength = 256;

    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasIndex(x => new { x.LanguageId, x.IsPublished, x.IsHidden, x.Order, x.Id });
        builder.HasIndex(x => new { x.LanguageId, x.Url, x.IsPublished });
        builder.HasIndex(x => x.LanguageId);
        builder.HasIndex(x => x.Order);

        builder.HasIndex(x => x.NormalizedTitle).GinTrigram();
        builder.GenerateNormalized(s => s.Title, t => t.NormalizedTitle);

        builder.Property(x => x.Title).HasMaxLength(TitleLength);
        builder.Property(x => x.Url).HasMaxLength(UrlLength);

        builder.HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Post)
            .WithOne()
            .HasForeignKey<Menu>(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParentMenu)
            .WithMany()
            .HasForeignKey(x => x.ParentMenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}