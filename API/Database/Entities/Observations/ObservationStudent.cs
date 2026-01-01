namespace API.Database.Entities.Observations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ObservationStudent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
}

public class ObservationStudentConfiguration : IEntityTypeConfiguration<ObservationStudent>
{
    public const int NameLength = 128;

    public void Configure(EntityTypeBuilder<ObservationStudent> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}