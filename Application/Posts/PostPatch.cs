namespace Application.Posts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Interfaces;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class PostPatch
    {
        public record Command(int Id, PostPatchDto Post) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;
            private readonly ISearchClient _search;
            private readonly IMapper _mapper;

            public Handler(DataContext context, ISearchClient search, IMapper mapper)
            {
                _context = context;
                _search = search;
                _mapper = mapper;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }

                _mapper.Map(request.Post, entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _search.UpdatePost(_mapper.Map(entity, new PostSearchDto()));

                return true;
            }
        }
    }
}