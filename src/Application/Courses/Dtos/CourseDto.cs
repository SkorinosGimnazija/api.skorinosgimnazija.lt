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
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Organizer { get; init; } = default!;

    public DateTime StartDate { get; init; } = default!;
     
    public DateTime EndDate { get; init; } = default!;
    public DateTime CreatedAt { get; init; } = default!;

    public float DurationInHours { get; init; }

    public string? CertificateNr { get; init; }

    public UserDto User { get; init; } = default!;
    public int UserId { get; init; } 
}
