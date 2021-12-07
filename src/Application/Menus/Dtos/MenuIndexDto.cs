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
        
    public bool IsPublished { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;

}