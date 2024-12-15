namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTargetCreateDto
{
    public string Name { get; init; } = null!;

    public bool Enabled { get; init; }
}