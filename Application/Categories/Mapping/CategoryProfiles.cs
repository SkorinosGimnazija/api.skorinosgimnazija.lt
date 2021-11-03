namespace Application.Categories.Mapping;

using AutoMapper;
using Domain.CMS;
using Dtos;

internal class CategoryProfiles : Profile
{
    public CategoryProfiles()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryEditDto, Category>();
    }
}