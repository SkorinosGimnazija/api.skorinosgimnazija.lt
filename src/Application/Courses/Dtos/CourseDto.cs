namespace SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Organizer { get; set; } = default!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int DurationInHours { get; set; }

    public string? CertificateNr { get; set; }

    public AppUser User { get; set; } = default!;
    public int UserId { get; set; } 
}
