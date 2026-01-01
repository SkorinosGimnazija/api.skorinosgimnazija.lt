namespace API.Endpoints.Menus.Public;

using JetBrains.Annotations;

[PublicAPI]
public record ListMenusPublicResponse
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public string? Url { get; init; }

    public required bool IsExternal { get; init; }

    public List<ListMenusPublicResponse>? Children { get; init; }
}