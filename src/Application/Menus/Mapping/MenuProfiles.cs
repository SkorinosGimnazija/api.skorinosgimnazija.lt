﻿namespace SkorinosGimnazija.Application.Menus.Mapping;

using AutoMapper;
using Domain.Entities;
using Dtos;
using SkorinosGimnazija.Application.Posts.Dtos;

public class MenuProfiles : Profile
{
    public MenuProfiles()
    {
        CreateMap<MenuLocation, MenuLocationDto>();

        CreateMap<Menu, MenuDto>();
        CreateMap<Menu, MenuDetailsDto>();

        CreateMap<MenuCreateDto, Menu>()
            .ForMember(x => x.Path, x => x.MapFrom(m => m.Slug));

        CreateMap<MenuEditDto, Menu>()
            .ForMember(x => x.Path, x => x.MapFrom(m => m.Slug));

        CreateMap<Menu, MenuIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));
    }
}