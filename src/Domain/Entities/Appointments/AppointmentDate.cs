namespace SkorinosGimnazija.Domain.Entities.Appointments;

public class AppointmentDate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public AppointmentType Type { get; set; } = default!;

    public int TypeId { get; set; }
}