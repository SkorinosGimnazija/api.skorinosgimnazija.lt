namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using School.Dtos;

public record TimetableDto
{
    public int Id { get; init; }

    public ClassdayDto Day { get; init; } = default!;

    public ClassroomDto Room { get; init; } = default!;

    public ClasstimeDto Time { get; init; } = default!;

    public string? ClassName { get; init; }
}
