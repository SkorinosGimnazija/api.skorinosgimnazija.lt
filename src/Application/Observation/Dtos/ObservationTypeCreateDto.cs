namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTypeCreateDto
{
    public string Name { get; init; } = null!;
}