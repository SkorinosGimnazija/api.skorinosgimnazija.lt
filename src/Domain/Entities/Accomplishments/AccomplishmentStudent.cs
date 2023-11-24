namespace SkorinosGimnazija.Domain.Entities.Accomplishments;

using School;

public class AccomplishmentStudent
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int AccomplishmentId { get; set; }

    public Accomplishment Accomplishment { get; set; } = default!;

    public int AchievementId { get; set; }

    public AccomplishmentAchievement Achievement { get; set; } = default!;

    public int ClassroomId { get; set; }

    public Classroom Classroom { get; set; } = default!;
}