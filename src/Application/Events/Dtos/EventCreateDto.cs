namespace SkorinosGimnazija.Application.Events.Dtos;

public record EventCreateDto
{
    public string Title { get; init; } = default!;

    public DateTime StartDate { get; init; } = default!;

    public DateTime EndDate { get; init; } = default!;

    public bool AllDay { get; init; }
}