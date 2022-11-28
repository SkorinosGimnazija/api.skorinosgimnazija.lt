namespace SkorinosGimnazija.Application.Appointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentReservedDateCreateDto
{
    public int DateId { get; init; }

    public string UserName { get; init; } = default!;
}