namespace SkorinosGimnazija.Application.Observation.Dtos;

public record ObservationTargetEditDto : ObservationTargetCreateDto
{
    public int Id { get; init; }
}