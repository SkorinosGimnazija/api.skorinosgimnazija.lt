namespace API.Database.Entities.FailureReports;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FailureReport
{
    public int Id { get; set; }

    public AppUser Creator { get; set; } = null!;

    public int CreatorId { get; set; }

    public string Location { get; set; } = null!;

    public string Details { get; set; } = null!;

    public DateTime ReportDate { get; set; }

    public bool? IsFixed { get; set; }

    public DateTime? FixDate { get; set; }
}

public class FailureReportConfiguration : IEntityTypeConfiguration<FailureReport>
{
    public const int LocationLength = 64;
    public const int DetailsLength = 512;

    public void Configure(EntityTypeBuilder<FailureReport> builder)
    {
        builder.HasIndex(x => x.ReportDate);
        builder.HasIndex(x => new { x.IsFixed, x.Id });

        builder.Property(x => x.Location).HasMaxLength(LocationLength);
        builder.Property(x => x.Details).HasMaxLength(DetailsLength);

        builder.HasOne(x => x.Creator).WithMany().HasForeignKey(x => x.CreatorId);
    }
}