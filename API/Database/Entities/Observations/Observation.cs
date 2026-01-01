namespace API.Database.Entities.Observations;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Observation
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public int CreatorId { get; set; }

    public int StudentId { get; set; }

    public int LessonId { get; set; }

    public string? Note { get; set; }

    public AppUser Creator { get; set; } = null!;

    public ObservationStudent Student { get; set; } = null!;

    public ObservationLesson Lesson { get; set; } = null!;

    public List<ObservationOption> Options { get; set; } = [];
}

public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
{
    public const int NoteLength = 512;

    public void Configure(EntityTypeBuilder<Observation> builder)
    {
        builder.HasIndex(x => new { x.StudentId, x.Date, x.Id });
        builder.HasIndex(x => new { x.CreatorId, x.Id });
        builder.HasIndex(x => new { x.Date, x.Id });
        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.CreatorId);

        builder.Property(x => x.Note).HasMaxLength(NoteLength);

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Lesson)
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Options)
            .WithMany(x => x.Observations)
            .UsingEntity(
                r => r.HasOne(typeof(ObservationOption))
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict),
                l => l.HasOne(typeof(Observation))
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade));
    }
}