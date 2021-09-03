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
            CreateMap<PostCreateDto, Post>();
            CreateMap<PostEditDto, Post>();

            CreateMap<PostPatchDto, Post>()
                .ForMember(x => x.IsFeatured, x => x.Condition(p => p.IsFeatured != null))
                .ForMember(x => x.IsPublished, x => x.Condition(p => p.IsPublished != null));
        }
    }
}