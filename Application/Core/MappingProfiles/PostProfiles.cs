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
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostDetailsDto>();
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostEditDto, Post>(); 
            CreateMap<PostPatchDto, Post>()
                .ForAllMembers(x => x.Condition((src, dest, member) => member != null));
            CreateMap<Post, PublicPostDto>()
                .ForMember(x => x.Url, x => x.MapFrom(p => $"/{p.Category.Slug}/{p.Id}/{p.Slug}"))
                .ForMember(x => x.Language, x => x.MapFrom(p => p.Category.Language.Slug));
            CreateMap<Post, PublicPostDetailsDto>();
        }
    }
}