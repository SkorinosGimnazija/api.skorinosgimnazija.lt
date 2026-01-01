namespace API.Database.Entities.Achievements;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AchievementType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class AchievementTypeConfiguration : IEntityTypeConfiguration<AchievementType>
{
    public const int NameLength = 64;

    public void Configure(EntityTypeBuilder<AchievementType> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}