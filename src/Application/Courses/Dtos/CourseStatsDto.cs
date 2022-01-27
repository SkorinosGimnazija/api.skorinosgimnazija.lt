namespace SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseStatsDto
{ 
    public int UserId { get; init; }
    public string UserDisplayName { get; init; } = default!;
    public float Hours { get; init; } 
    public int Count { get; init; } 
    public int UsefulCount { get; init; } 
    public float Price { get; init; } 
    public DateTime LastUpdate { get; init; }
     
}
