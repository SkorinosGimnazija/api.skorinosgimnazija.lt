namespace API.Database.Entities.Observations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ObservationLesson
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class ObservationLessonConfiguration : IEntityTypeConfiguration<ObservationLesson>
{
    public const int NameLength = 128;

    public void Configure(EntityTypeBuilder<ObservationLesson> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}