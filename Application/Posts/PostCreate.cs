using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Posts.Dtos;
using Application.Posts.Validation;
using AutoMapper;
using Domain.CMS;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace Application.Posts;


    public class PostCreate
    {
        public record Command(PostCreateDto Post) : IRequest<PostDetailsDto>;

        public class Handler : IRequestHandler<Command, PostDetailsDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PostDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = _mapper.Map(request.Post, new Post());

                _context.Posts.Add(post);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map(post, new PostDetailsDto()); 
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