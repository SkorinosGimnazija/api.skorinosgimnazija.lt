namespace SkorinosGimnazija.Domain.Entities.Appointments;

public class AppointmentReservedDate
{
    public int Id { get; set; }

    public AppointmentDate Date { get; set; } = default!;

    public int DateId { get; set; }

    public string UserName { get; set; } = default!;
}