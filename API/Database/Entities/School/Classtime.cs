namespace API.Database.Entities.School;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Classtime
{
    public int Id { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly StartTimeShort { get; set; }

    public TimeOnly EndTime { get; set; }

    public TimeOnly EndTimeShort { get; set; }
}

internal class ClasstimeConfiguration : IEntityTypeConfiguration<Classtime>
{
    public void Configure(EntityTypeBuilder<Classtime> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}