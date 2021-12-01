namespace SkorinosGimnazija.Application.Banners.Mapping;

using AutoMapper;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos;

public class BannerProfiles : Profile
{
    public BannerProfiles()
    {
        CreateMap<Banner, BannerDto>();

        CreateMap<BannerEditDto, Banner>();

        CreateMap<BannerCreateDto, Banner>();
    }
}
