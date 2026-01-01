namespace API.Endpoints.Observations.ObservationLessons;

using JetBrains.Annotations;

[PublicAPI]
public record ObservationLessonResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}