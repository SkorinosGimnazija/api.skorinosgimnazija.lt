namespace SkorinosGimnazija.Domain.Entities.CMS;

public sealed class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}