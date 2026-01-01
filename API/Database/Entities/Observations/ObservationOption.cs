namespace API.Database.Entities.Observations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ObservationOption
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Observation> Observations { get; set; } = [];
}

public class ObservationOptionConfiguration : IEntityTypeConfiguration<ObservationOption>
{
    public const int NameLength = 256;

    public void Configure(EntityTypeBuilder<ObservationOption> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}