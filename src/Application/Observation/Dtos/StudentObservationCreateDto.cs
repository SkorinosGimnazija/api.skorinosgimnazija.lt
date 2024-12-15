namespace SkorinosGimnazija.Application.Observation.Dtos;

public record StudentObservationCreateDto
{
    public string? Note { get; init; }
    
    public DateOnly Date { get; init; }
    
    public int TargetId { get; init; }
    
    public int LessonId { get; init; }

    public List<int> TypeIds { get; init; } = [];

}