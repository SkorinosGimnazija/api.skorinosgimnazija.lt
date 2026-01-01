namespace API.Endpoints.School.Classrooms;

using JetBrains.Annotations;

[PublicAPI]
public record ClassroomResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}