namespace API.Endpoints.Courses.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListCoursesRequest : PaginationRequest
{
    [QueryParam]
    public int? UserId { get; init; }
}