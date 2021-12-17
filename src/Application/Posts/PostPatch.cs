﻿namespace SkorinosGimnazija.Application.Posts;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Domain.Entities;

public static class PostPatch
{
    public record Command(int Id, PostPatchDto Post) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ISearchClient _searchClient;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMapper mapper)
        {
            _context = context;
            _searchClient = searchClient;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.Post, entity);

            await _searchClient.SavePostAsync(_mapper.Map<PostIndexDto>(entity));
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}