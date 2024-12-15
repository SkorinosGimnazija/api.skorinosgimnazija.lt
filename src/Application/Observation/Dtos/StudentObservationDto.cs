namespace SkorinosGimnazija.Application.Observation.Dtos;

using Common.Dtos;

public record StudentObservationDto
{
    public int Id { get; init; }
    
    public string? Note { get; init; }
    
    public DateOnly Date { get; init; }
    
    public IdNameDto Target { get; init; } = null!;

    public IdNameDto Teacher { get; init; } = null!;

    public IdNameDto Lesson { get; init; } = null!;
    
    public List<ObservationTypeDto> Types { get; init; }  = null!;
}