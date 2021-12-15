namespace SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record EventDto
{
    public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? StartDate { get; init; } 
    public string? StartDateTime { get; init; }
    public string? EndDate { get; init; }
    public string? EndDateTime { get; init; }
}
