namespace API.Endpoints.School.ShortDays.Create;

using JetBrains.Annotations;

[PublicAPI]
public record CreateShortDayRequest
{
    public required DateOnly Date { get; init; }
}