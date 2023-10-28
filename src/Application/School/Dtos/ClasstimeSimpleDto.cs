namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ClasstimeSimpleDto
{
    public int Number { get; init; }

    public string StartTime { get; init; } = default!;

    public string EndTime { get; init; } = default!;
}
