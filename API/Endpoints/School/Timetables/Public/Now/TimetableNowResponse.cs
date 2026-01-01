namespace API.Endpoints.School.Timetables.Public.Now;

using JetBrains.Annotations;

[PublicAPI]
public record TimetableNowResponse
{
    public required string CurrentTime { get; init; }

    public required Class CurrentClass { get; init; }

    public required List<RoomClass> RoomClasses { get; init; }

    [PublicAPI]
    public record RoomClass
    {
        public required int RoomId { get; init; }

        public required string RoomName { get; init; }

        public required string ClassName { get; init; }
    }

    [PublicAPI]
    public record Class
    {
        public required int Id { get; init; }

        public required string StartTime { get; init; }

        public required string EndTime { get; init; }
    }
}