namespace SkorinosGimnazija.Application.Events.Dtos;

public record EventDto
{
    public string Id { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string? StartDate { get; init; }

    public string? StartDateTime { get; init; }

    public string? EndDate { get; init; }

    public string? EndDateTime { get; init; }
}