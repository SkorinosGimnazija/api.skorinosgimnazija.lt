namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ClasstimeCreateDto
{
    public int Number { get; init; }

    public TimeOnly StartTime { get; init; }

    public TimeOnly StartTimeShort { get; init; }

    public TimeOnly EndTime { get; init; }

    public TimeOnly EndTimeShort { get; init; }
}
