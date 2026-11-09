namespace API.Endpoints.Posts.Public.Search;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

[PublicAPI]
public record SearchPostsPublicRequest : PaginationRequest
{
    [RouteParam]
    public required string LanguageId { get; init; }

    [QueryParam]
    [Length(3, 256)]
    public required string SearchTerm { get; init; }
}