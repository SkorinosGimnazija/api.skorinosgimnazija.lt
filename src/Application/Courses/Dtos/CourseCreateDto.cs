namespace SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseCreateDto
{
    public string Name { get; set; } = default!;

    public string Organizer { get; set; } = default!;

    public DateTime StartDate { get; set; } = default!;

    public DateTime EndDate { get; set; } = default!;

    public int DurationInHours { get; set; }

    public string? CertificateNr { get; set; }
}
