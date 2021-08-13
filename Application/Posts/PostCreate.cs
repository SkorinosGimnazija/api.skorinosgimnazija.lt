﻿namespace Application.Posts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;
    using Validation;

    public class PostCreate
    {
        public record Command(PostCreateDto Post) : IRequest<ActionResult<Post>>;

        public class Handler : IRequestHandler<Command, ActionResult<Post>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Post>> Handle(Command request, CancellationToken cancellationToken)
            {
                var post = _mapper.Map(request.Post, new Post());

                _context.Posts.Add(post);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(post) { StatusCode = StatusCodes.Status201Created };
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