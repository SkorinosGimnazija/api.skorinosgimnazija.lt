namespace API.Endpoints.School.Classtimes.Upsert;

using JetBrains.Annotations;

[PublicAPI]
public record UpsertClasstimeRequest
{
    public required int Id { get; init; }

    public required TimeOnly StartTime { get; init; }

    public required TimeOnly StartTimeShort { get; init; }

    public required TimeOnly EndTime { get; init; }

    public required TimeOnly EndTimeShort { get; init; }
}