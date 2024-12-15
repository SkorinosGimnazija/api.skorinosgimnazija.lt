namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationLessonCreateDto
{
    public string Name { get; init; } = null!;
}