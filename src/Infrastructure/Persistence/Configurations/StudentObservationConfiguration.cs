namespace SkorinosGimnazija.Infrastructure.Persistence.Configurations;

using Domain.Entities.Observation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class StudentObservationConfiguration : IEntityTypeConfiguration<StudentObservation>
{
    public void Configure(EntityTypeBuilder<StudentObservation> builder)
    {
        builder.HasOne(x => x.Target).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Lesson).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Types)
            .WithMany(x => x.StudentObservations)
            .UsingEntity(
                l => l.HasOne(typeof(ObservationType)).WithMany().OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne(typeof(StudentObservation)).WithMany().OnDelete(DeleteBehavior.Cascade));

        builder.Property(x => x.Note).HasMaxLength(500);
    }
}

internal class ObservationTypeConfiguration : IEntityTypeConfiguration<ObservationType>
{
    public void Configure(EntityTypeBuilder<ObservationType> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255);
    }
}

internal class ObservationTargetConfiguration : IEntityTypeConfiguration<ObservationTarget>
{
    public void Configure(EntityTypeBuilder<ObservationTarget> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255);
    }
}

internal class ObservationLessonConfiguration : IEntityTypeConfiguration<ObservationLesson>
{
    public void Configure(EntityTypeBuilder<ObservationLesson> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255);
    }
}