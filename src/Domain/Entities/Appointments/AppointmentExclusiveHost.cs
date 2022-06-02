namespace SkorinosGimnazija.Domain.Entities.Appointments;

public class AppointmentExclusiveHost
{
    public int Id { get; set; }

    public AppointmentType Type { get; set; } = default!;

    public int TypeId { get; set; }

    public string UserName { get; set; } = default!;
}