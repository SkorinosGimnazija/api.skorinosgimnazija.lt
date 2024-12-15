namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationLessonEditDto : ObservationLessonCreateDto
{
    public int Id { get; init; }
}
