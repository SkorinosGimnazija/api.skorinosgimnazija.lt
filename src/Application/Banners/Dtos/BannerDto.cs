namespace SkorinosGimnazija.Application.Banners.Dtos;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record BannerDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;

    public bool IsPublished { get; init; }

    public string PictureUrl { get; init; } = default!;

    public int Order { get; init; }

    public int LanguageId { get; init; }
}
