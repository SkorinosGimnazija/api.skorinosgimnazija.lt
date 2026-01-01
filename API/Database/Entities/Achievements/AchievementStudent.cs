namespace API.Database.Entities.Achievements;

using API.Database.Entities.School;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AchievementStudent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AchievementId { get; set; }

    public Achievement Achievement { get; set; } = null!;

    public int AchievementTypeId { get; set; }

    public AchievementType AchievementType { get; set; } = null!;

    public int ClassroomId { get; set; }

    public Classroom Classroom { get; set; } = null!;
}

public class AchievementStudentConfiguration : IEntityTypeConfiguration<AchievementStudent>
{
    public const int NameLength = 256;

    public void Configure(EntityTypeBuilder<AchievementStudent> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(NameLength);

        builder.HasOne(x => x.Classroom)
            .WithMany()
            .HasForeignKey(x => x.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}