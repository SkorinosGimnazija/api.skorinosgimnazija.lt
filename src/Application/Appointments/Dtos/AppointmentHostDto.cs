namespace SkorinosGimnazija.Application.Appointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentHostDto
{
    public string DisplayName { get; init; } = default!;

    public string UserName { get; init; } = default!;
}