namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using Domain.Entities.Timetable;
using School.Dtos;

public record TimetablePublicDto
{
    public List<TimetableSimpleDto> Timetable { get; init; } = default!;

    public ClasstimeSimpleDto Classtime { get; init; } = default!;

    public string CurrentTime { get; init; } = default!;
}
