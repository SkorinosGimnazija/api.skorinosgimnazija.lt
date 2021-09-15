namespace Application.Posts
{

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Application.Posts.Dtos;
    using Application.Posts.Validation;
    using AutoMapper;
    using Domain.CMS;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;



    public class PostCreate
    {
        public record Command(PostCreateDto Post) : IRequest<PostDetailsDto>;

        public class Handler : IRequestHandler<Command, PostDetailsDto>
        {
            private readonly DataContext _context;
            private readonly ISearchClient _searchClient;
            private readonly IFileManager _fileManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, ISearchClient searchClient, IFileManager fileManager, IMapper mapper)
            {
                _context = context;
                _searchClient = searchClient;
                _fileManager = fileManager;
                _mapper = mapper;
            }

            public async Task<PostDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Post, new Post());

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
               
                if (request.Post.Images.Any())
                {
                    entity.Images = await _fileManager.SaveImagesAsync(entity.Id, request.Post.Images);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await _searchClient.SavePost(_mapper.Map(entity, new PostSearchDto()));

                return _mapper.Map(entity, new PostDetailsDto());
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Post).SetValidator(new PostCreateValidator());
                }
            }
        }
    }
}