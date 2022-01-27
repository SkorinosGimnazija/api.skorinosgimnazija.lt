namespace SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseStats
{
    public string Name { get; init; } = default!;
    public float Hours { get; init; }
    public float? Price { get; init; }
    public DateTime LastUpdate { get; init; }
}
