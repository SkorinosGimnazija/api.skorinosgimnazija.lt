namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentEditDto : AccomplishmentCreateDto
{
    public int Id { get; init; }
}