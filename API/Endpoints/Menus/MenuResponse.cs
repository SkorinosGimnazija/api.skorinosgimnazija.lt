namespace API.Endpoints.Menus;

using JetBrains.Annotations;

[PublicAPI]
public record MenuResponse
{
    public required int Id { get; init; }

    public required int Order { get; init; }

    public required bool IsPublished { get; init; }

    public required bool IsHidden { get; init; }

    public required string Title { get; init; }

    public string? Url { get; init; }

    public int? PostId { get; init; }

    public string? PostTitle { get; init; }

    public required string LanguageName { get; init; }

    public required string LanguageId { get; set; }

    public int? ParentMenuId { get; init; }

    public List<MenuResponse>? Children { get; init; }
}