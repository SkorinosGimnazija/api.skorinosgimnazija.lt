namespace API.Endpoints.Observations.ObservationStudents;

using JetBrains.Annotations;

[PublicAPI]
public record ObservationStudentResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required bool IsActive { get; init; }
}