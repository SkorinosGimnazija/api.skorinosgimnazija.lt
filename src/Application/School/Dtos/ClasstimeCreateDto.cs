namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ClasstimeCreateDto
{
    public string Name { get; init; } = default!;

    public TimeOnly StartTime { get; init; }

    public TimeOnly EndTime { get; init; }
}
