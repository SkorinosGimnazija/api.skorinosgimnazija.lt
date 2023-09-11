namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using School.Dtos;

public record TimetableSimpleDto
{
    public int Id { get; init; }

    public string ClassRoom { get; init; } = default!;

    public string ClassName { get; init; } = default!;
}
