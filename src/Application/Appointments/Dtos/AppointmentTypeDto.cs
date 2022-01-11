namespace SkorinosGimnazija.Application.ParentAppointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentTypeDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public int DurationInMinutes { get; init; }
    public bool InvitePrincipal { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public DateTime RegistrationEnd { get; init; }

}
