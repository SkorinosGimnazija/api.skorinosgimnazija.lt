namespace SkorinosGimnazija.Application.Posts;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PublicPostSearchList
{
    public record Query(string SearchText, PaginationDto Pagination) : IRequest<List<PostDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).SetValidator(new PaginationValidator());
            RuleFor(x => x.SearchText).MinimumLength(3).MaximumLength(50);
        }
    }

    public class Handler : IRequestHandler<Query, List<PostDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISearchClient _search;

        public Handler(IAppDbContext context, ISearchClient search, IMapper mapper)
        {
            _context = context;
            _search = search;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var postIds = await _search.SearchPostAsync(request.SearchText, cancellationToken);

            return await _context.Posts
                       .AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.PublishDate <= DateTime.UtcNow &&
                           postIds.Contains(x.Id))
                       .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => postIds.IndexOf(x.Id))
                       .ToListAsync(cancellationToken);

            //return await _context.Posts
            //    .AsNoTracking()
            //    .Where(x =>
            //        x.IsPublished &&
            //        x.PublishDate <= DateTime.UtcNow &&
            //        EF.Functions.ILike(x.Title, $"%{request.SearchText}%"))
            //    .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            //    .OrderByDescending(x => x.PublishDate)
            //    .Paginate(request.Pagination)
            //    .ToListAsync(cancellationToken);
        }
    }
}