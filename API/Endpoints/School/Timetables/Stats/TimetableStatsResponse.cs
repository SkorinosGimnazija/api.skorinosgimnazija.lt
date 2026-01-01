namespace API.Endpoints.School.Timetables.Stats;

using JetBrains.Annotations;

[PublicAPI]
public record TimetableStatsResponse
{
    public required int RoomId { get; init; }

    public required string RoomName { get; init; }

    public required Dictionary<int, int> CountsByDay { get; init; }

    public required List<DateOnly> OverrideDates { get; init; }
}