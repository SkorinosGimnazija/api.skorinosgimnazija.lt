namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationLessonDto
{
    public int Id { get; init; }
    
    public string Name { get; init; } = null!;
}