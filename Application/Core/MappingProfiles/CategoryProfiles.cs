namespace Application.Core.MappingProfiles
{
    using AutoMapper;
    using Domain.CMS;
    using Menus.Dtos;

    internal class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, Category>();
        }
    }
}