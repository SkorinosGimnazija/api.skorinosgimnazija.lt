namespace SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppointmentType
{
    public int Id { get; set; }

    public int DurationInMinutes { get; set; }

    public bool InvitePrincipal { get; set; }

    public bool IsPublic { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime RegistrationEnd { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}
