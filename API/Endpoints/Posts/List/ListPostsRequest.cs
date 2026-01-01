namespace API.Endpoints.Posts.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListPostsRequest : PaginationRequest
{
    [QueryParam]
    public string? SearchTerm { get; init; }
}