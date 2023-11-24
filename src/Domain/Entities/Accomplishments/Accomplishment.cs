namespace SkorinosGimnazija.Domain.Entities.Accomplishments;

using Identity;

public class Accomplishment
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public DateOnly Date { get; set; }

    public int UserId { get; set; }

    public AppUser User { get; set; } = default!;

    public int ScaleId { get; set; }

    public AccomplishmentScale Scale { get; set; } = default!;

    public ICollection<AccomplishmentTeacher> AdditionalTeachers { get; set; } = default!;

    public ICollection<AccomplishmentStudent> Students { get; set; } = default!;
}