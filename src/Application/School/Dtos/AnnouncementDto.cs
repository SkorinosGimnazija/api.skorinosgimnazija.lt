namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AnnouncementDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public DateOnly StartTime { get; init; }

    public DateOnly EndTime { get; init; }
}
