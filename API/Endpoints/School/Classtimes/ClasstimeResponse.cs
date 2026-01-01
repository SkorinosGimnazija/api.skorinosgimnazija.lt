namespace API.Endpoints.School.Classtimes;

using JetBrains.Annotations;

[PublicAPI]
public record ClasstimeResponse
{
    public required int Id { get; init; }

    public required string StartTime { get; init; }

    public required string StartTimeShort { get; init; }

    public required string EndTime { get; init; }

    public required string EndTimeShort { get; init; }
}