namespace SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseCreateDto
{
    public string Title { get; init; } = default!;

    public string Organizer { get; init; } = default!;

    public DateTime StartDate { get; init; } = default!;

    public DateTime EndDate { get; init; } = default!;
     
    public float DurationInHours { get; init; }

    public string? CertificateNr { get; init; }
}
