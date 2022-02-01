﻿namespace SkorinosGimnazija.Application.Banners.Mapping;

using AutoMapper;
using Domain.Entities;
using Dtos;

public class BannerProfiles : Profile
{
    public BannerProfiles()
    {
        CreateMap<Banner, BannerDto>();

        CreateMap<BannerEditDto, Banner>();

        CreateMap<BannerCreateDto, Banner>();

        CreateMap<Banner, BannerIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));
    }
}