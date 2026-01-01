namespace API.Endpoints.Courses.Update;

using API.Endpoints.Courses.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateCourseRequest : CreateCourseRequest
{
    public required int Id { get; init; }
}