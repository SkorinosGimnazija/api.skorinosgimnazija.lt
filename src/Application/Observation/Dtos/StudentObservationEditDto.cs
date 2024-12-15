namespace SkorinosGimnazija.Application.Observation.Dtos;

public record StudentObservationEditDto : StudentObservationCreateDto
{
    public int Id { get; init; }
}