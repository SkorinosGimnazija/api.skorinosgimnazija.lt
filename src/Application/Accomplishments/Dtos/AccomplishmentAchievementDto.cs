namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentAchievementDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;
}