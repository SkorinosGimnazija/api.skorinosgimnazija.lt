namespace API.Database.Entities.School;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShortDay
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }
}

internal class ClasstimeShortDayConfiguration : IEntityTypeConfiguration<ShortDay>
{
    public void Configure(EntityTypeBuilder<ShortDay> builder)
    {
        builder.HasIndex(x => x.Date).IsUnique();
    }
}