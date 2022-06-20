namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentScaleDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;
}