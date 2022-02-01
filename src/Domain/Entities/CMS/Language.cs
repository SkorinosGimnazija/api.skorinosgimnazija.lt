namespace SkorinosGimnazija.Domain.Entities;

public sealed class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}