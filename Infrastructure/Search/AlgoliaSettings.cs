namespace Infrastructure.Search;

public record AlgoliaSettings
{
    public string AppId { get; set; } = default!;

    public string ApiKey { get; set; } = default!;
}