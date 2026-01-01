namespace API.Endpoints.Banners.Public;

using JetBrains.Annotations;

[PublicAPI]
public sealed class ListBannersPublicRequest
{
    [RouteParam]
    public required string LanguageId { get; init; }
}