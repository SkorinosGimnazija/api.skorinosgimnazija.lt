namespace API.Database.Entities.School;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Classday
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class ClassdayConfiguration : IEntityTypeConfiguration<Classday>
{
    public void Configure(EntityTypeBuilder<Classday> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(64);
    }
}