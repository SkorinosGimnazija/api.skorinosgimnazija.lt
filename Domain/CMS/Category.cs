namespace Domain.CMS;

public class Category
{
    public int Id { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public Language Language { get; set; } = default!;

    public bool ShowOnHomePage { get; set; }
}