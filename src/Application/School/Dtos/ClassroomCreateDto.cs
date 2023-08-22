namespace SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ClassroomCreateDto
{
    public string Name { get; init; } = default!;

    public int Number { get; init; }
}
