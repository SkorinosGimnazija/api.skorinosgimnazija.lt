namespace SkorinosGimnazija.Domain.Entities.Observation;

using Identity;

public sealed class StudentObservation
{
    public int Id { get; set; }

    public string? Note { get; set; }

    public DateOnly Date { get; set; }

    public ObservationTarget Target { get; set; } = null!;

    public int TargetId { get; set; }

    public AppUser Teacher { get; set; } = null!;

    public int TeacherId { get; set; }

    public ObservationLesson Lesson { get; set; } = null!;

    public int LessonId { get; set; }

    public List<ObservationType> Types { get; set; } = [];
}