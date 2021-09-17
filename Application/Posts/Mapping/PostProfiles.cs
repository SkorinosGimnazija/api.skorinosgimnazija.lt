namespace Application.Core.MappingProfiles
{
    using System;
    using AutoMapper;
    using Domain.CMS;
    using Posts.Dtos;

    public class PostProfiles : Profile
    {
        public PostProfiles()
        {
            CreateMap<Post, PostDto>();
            CreateMap<Post, PostDetailsDto>();
            CreateMap<PostEditDto, Post>();

            CreateMap<PostCreateDto, Post>()
                .ForMember(x => x.Images, x => x.Ignore())
                .ForMember(x => x.Files, x => x.Ignore());

            CreateMap<Post, PostSearchDto>().ForMember(x=> x.ObjectID, x=> x.MapFrom(p=> p.Id.ToString()));

            CreateMap<PostPatchDto, Post>()
                .ForMember(x => x.IsFeatured, x => x.Condition(p => p.IsFeatured != null))
                .ForMember(x => x.IsPublished, x => x.Condition(p => p.IsPublished != null));
        }
    }
}