namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ClasstimeShortDayDto
{
    public int Id { get; init; }

    public DateOnly Date { get; init; }
}
