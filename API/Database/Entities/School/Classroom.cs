namespace API.Database.Entities.School;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Classroom
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public const int NameLength = 64;

    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}