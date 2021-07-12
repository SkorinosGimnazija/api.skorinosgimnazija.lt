namespace Application.Core
{
    using AutoMapper;
    using Domain.CMS;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, Post>();
        }
    }
}