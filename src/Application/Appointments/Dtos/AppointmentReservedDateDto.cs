namespace SkorinosGimnazija.Application.Appointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentReservedDateDto
{
    public int Id { get; init; }
    public int DateId { get; init; }
}
