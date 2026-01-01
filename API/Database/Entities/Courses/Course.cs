namespace API.Database.Entities.Courses;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Organizer { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public double DurationInHours { get; set; }

    public string? Certificate { get; set; }

    public bool IsUseful { get; set; }

    public int CreatorId { get; set; }

    public AppUser Creator { get; set; } = null!;
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public const int TitleLength = 256;
    public const int OrganizerLength = 256;
    public const int CertificateLength = 128;

    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasIndex(x => new { x.StartDate, x.Id });
        builder.HasIndex(x => x.StartDate);
        builder.HasIndex(x => x.EndDate);
        builder.HasIndex(x => x.CreatorId);

        builder.Property(x => x.Title).HasMaxLength(TitleLength);
        builder.Property(x => x.Organizer).HasMaxLength(OrganizerLength);
        builder.Property(x => x.Certificate).HasMaxLength(CertificateLength);

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}