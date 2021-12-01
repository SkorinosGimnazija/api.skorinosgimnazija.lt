namespace SkorinosGimnazija.Application.Banners.Dtos;

using Microsoft.AspNetCore.Http;
using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record BannerEditDto : BannerCreateDto
{
    public int Id { get; init; } 
    public new IFormFile? Picture { get; init; } = default!;

}
