namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentCreateDto
{
    public string Name { get; init; } = default!;

    public string Achievement { get; init; } = default!;

    public DateTime Date { get; init; }

    public int ScaleId { get; init; }

    public ICollection<string> AdditionalTeachers { get; init; } = new List<string>();

    public ICollection<string> Students { get; init; } = new List<string>();
}