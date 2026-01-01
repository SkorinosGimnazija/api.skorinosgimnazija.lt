namespace API.Database.Entities.Achievements;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AchievementScale
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class AchievementScaleConfiguration : IEntityTypeConfiguration<AchievementScale>
{
    public const int NameLength = 64;

    public void Configure(EntityTypeBuilder<AchievementScale> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(NameLength);
    }
}