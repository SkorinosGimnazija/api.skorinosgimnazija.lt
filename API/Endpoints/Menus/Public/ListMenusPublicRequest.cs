namespace API.Endpoints.Menus.Public;

using JetBrains.Annotations;

[PublicAPI]
public sealed class ListMenusPublicRequest
{
    [RouteParam]
    public required string LanguageId { get; init; }
}