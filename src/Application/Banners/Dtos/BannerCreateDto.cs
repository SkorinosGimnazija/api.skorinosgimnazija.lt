namespace SkorinosGimnazija.Application.Banners.Dtos;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public record BannerCreateDto
{
    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;

    public bool IsPublished { get; init; }
     
    public IFormFile Picture { get; init; } = default!;

    public int Order { get; init; }

    public int LanguageId { get; init; }
}
