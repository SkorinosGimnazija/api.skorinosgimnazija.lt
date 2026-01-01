namespace API.Endpoints.Menus.List;

using JetBrains.Annotations;

[PublicAPI]
public sealed class ListMenusRequest
{
    [QueryParam]
    public string? LanguageId { get; init; }
}