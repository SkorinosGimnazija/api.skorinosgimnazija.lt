namespace API.Database.Entities.BullyReports;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BullyReport
{
    public int Id { get; set; }

    public bool IsPublicReport { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateOnly Date { get; set; }

    public string VictimName { get; set; } = null!;

    public string BullyName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Details { get; set; } = null!;

    public string? Observers { get; set; }

    public string? Actions { get; set; }

    public AppUser? Creator { get; set; }

    public int? CreatorId { get; set; }
}

public class BullyReportConfiguration : IEntityTypeConfiguration<BullyReport>
{
    public const int NameLength = 256;
    public const int LocationLength = 128;
    public const int DetailsLength = 1024;

    public void Configure(EntityTypeBuilder<BullyReport> builder)
    {
        builder.HasIndex(x => new { x.CreatorId, x.CreatedAt });
        builder.HasIndex(x => x.CreatorId);
        builder.HasIndex(x => x.CreatedAt);

        builder.Property(x => x.Location).HasMaxLength(LocationLength);
        builder.Property(x => x.VictimName).HasMaxLength(NameLength);
        builder.Property(x => x.BullyName).HasMaxLength(NameLength);
        builder.Property(x => x.Observers).HasMaxLength(DetailsLength);
        builder.Property(x => x.Details).HasMaxLength(DetailsLength);
        builder.Property(x => x.Actions).HasMaxLength(DetailsLength);

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);
    }
}