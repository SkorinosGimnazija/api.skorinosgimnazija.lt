namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTypeDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
}