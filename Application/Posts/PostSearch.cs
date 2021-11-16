﻿namespace Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Dtos;
using Core.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

public static class PostSearch
{
    public record Query(string SearchText, PaginationDto Pagination) : IRequest<List<PostDto>>;

    public class Handler : IRequestHandler<Query, List<PostDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ISearchClient _search;

        public Handler(DataContext context, ISearchClient search, IMapper mapper)
        {
            _context = context;
            _search = search;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var postIds = await _search.SearchPost(request.SearchText, cancellationToken);

            return await _context.Posts
                .AsNoTracking()
                .Where(x => postIds.Contains(x.Id))
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .OrderBy(x => postIds.IndexOf(x.Id))
                .ToListAsync(cancellationToken);

            //return await _context.Posts
            //    .AsNoTracking()
            //    .Where(x => EF.Functions.ILike(x.Title, $"%{request.SearchText}%"))
            //    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            //    .OrderByDescending(x => x.PublishDate)
            //    .Paginate(request.Pagination)
            //    .ToListAsync(cancellationToken);
        }
    }
}