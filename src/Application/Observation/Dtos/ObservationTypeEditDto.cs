namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTypeEditDto : ObservationTypeCreateDto
{
    public int Id { get; init; }
}