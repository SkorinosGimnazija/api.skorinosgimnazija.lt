namespace API.Database.Entities.Achievements;

using API.Database.Entities.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int CreatorId { get; set; }

    public AppUser Creator { get; set; } = null!;

    public int ScaleId { get; set; }

    public AchievementScale Scale { get; set; } = null!;

    public List<AchievementStudent> Students { get; set; } = [];

    public List<AppUser> AdditionalTeachers { get; set; } = [];
}

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public const int NameLength = 256;

    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => x.CreatorId);
        builder.HasIndex(x => new { x.Date, x.Id });

        builder.Property(x => x.Name).HasMaxLength(NameLength);

        builder.HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Scale)
            .WithMany()
            .HasForeignKey(x => x.ScaleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Students)
            .WithOne(x => x.Achievement)
            .HasForeignKey(x => x.AchievementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AdditionalTeachers)
            .WithMany();
    }
}