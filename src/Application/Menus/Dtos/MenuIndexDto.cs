namespace SkorinosGimnazija.Application.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record MenuIndexDto
{
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;
        
    public string Title { get; init; } = default!;
    public string Path { get; init; } = default!;
    public string? Url { get; set; }
}