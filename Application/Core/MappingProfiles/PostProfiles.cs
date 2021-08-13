namespace Application.Core.MappingProfiles
{
    using AutoMapper;
    using Domain.CMS;
    using Posts.Dtos;

    public class PostProfiles : Profile
    {
        public PostProfiles()
        {
            CreateMap<Post, Post>();
            CreateMap<PostEditDto, Post>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<Post, PostDto>()
               .ForMember(x => x.Url, x => x.MapFrom(p => $"/{p.Category.Slug}/{p.Id}/{p.Slug}"))
               .ForMember(x => x.Language, x => x.MapFrom(p => p.Category.Language.Slug));
        }
    }   
}