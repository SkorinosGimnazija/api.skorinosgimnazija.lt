namespace SkorinosGimnazija.Infrastructure.Options;

public record AlgoliaOptions
{
    public string AppId { get; set; } = default!;

    public string ApiKey { get; set; } = default!;
}