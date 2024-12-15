namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTargetDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public bool Enabled { get; init; }
}