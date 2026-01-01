namespace API.Endpoints.Posts.Public.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListPostsPublicRequest : PaginationRequest
{
    [RouteParam]
    public required string LanguageId { get; init; }
}