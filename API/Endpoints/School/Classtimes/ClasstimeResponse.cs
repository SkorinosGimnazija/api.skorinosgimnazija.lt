namespace API.Endpoints.School.Classtimes;

using JetBrains.Annotations;

[PublicAPI]
public record ClasstimeResponse
{
    public required int Id { get; init; }

    public required string StartTime { get; init; }

    public string? StartTimeShort { get; init; }

    public required string EndTime { get; init; }

    public string? EndTimeShort { get; init; }
}