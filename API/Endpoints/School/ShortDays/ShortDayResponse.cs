namespace API.Endpoints.School.ShortDays;

using JetBrains.Annotations;

[PublicAPI]
public record ShortDayResponse
{
    public required int Id { get; init; }

    public required DateOnly Date { get; init; }
}