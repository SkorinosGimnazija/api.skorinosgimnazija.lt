namespace SkorinosGimnazija.Infrastructure.Options;

public record MediaManagerOptions
{
    public string[] UploadPath { get; set; } = default!;
}