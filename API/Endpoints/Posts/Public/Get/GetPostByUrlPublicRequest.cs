namespace API.Endpoints.Posts.Public.Get;

using JetBrains.Annotations;

[PublicAPI]
public record GetPostByUrlPublicRequest
{
    [RouteParam]
    public required string MenuUrl { get; init; }

    [RouteParam]
    public required string LanguageId { get; init; }
}