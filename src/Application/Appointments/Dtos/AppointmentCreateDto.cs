namespace SkorinosGimnazija.Application.Appointments.Dtos;

using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentPublicCreateDto
{
    public string CaptchaToken { get; init; } = default!;
    public int DateId { get; set; }
    public string UserName { get; set; } = default!;
    public string AttendeeName { get; set; } = default!;
    public string AttendeeEmail { get; set; } = default!;

}
