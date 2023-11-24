namespace SkorinosGimnazija.Domain.Entities.Accomplishments;

public class AccomplishmentTeacher
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int AccomplishmentId { get; set; }

    public Accomplishment Accomplishment { get; set; } = default!;
}