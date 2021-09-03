﻿namespace Application.Core.MappingProfiles
{
using Application.Categories.Dtos;
    using AutoMapper;
    using Domain.CMS;
    using Menus.Dtos;

    internal class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryEditDto, Category>();
        }
    }
}