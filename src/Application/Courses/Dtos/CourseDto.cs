namespace SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Common.Dtos;
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

    public string StartDate { get; set; } = default!;

    public string EndDate { get; set; } = default!;

    public int DurationInHours { get; set; }

    public string? CertificateNr { get; set; }

    public AppUserDto User { get; set; } = default!;
    public int UserId { get; set; } 
}
