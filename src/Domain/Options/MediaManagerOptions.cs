namespace SkorinosGimnazija.Domain.Options;

public record MediaManagerOptions
{
    public string[] UploadPath { get; set; } = default!;
}