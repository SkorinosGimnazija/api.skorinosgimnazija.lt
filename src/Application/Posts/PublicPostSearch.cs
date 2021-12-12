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
    public record Query(string SearchText, PaginationDto Pagination) : IRequest<PaginatedList<PostDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
            RuleFor(x => x.SearchText).MinimumLength(3).MaximumLength(50);
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<PostDto>>
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

        public async Task<PaginatedList<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchResult = await _search.SearchPostAsync(request.SearchText, request.Pagination, cancellationToken);
            var items = await _context.Posts
                            .AsNoTracking()
                            .Where(x =>
                                x.IsPublished &&
                                x.PublishDate <= DateTime.UtcNow &&
                                searchResult.Items.Contains(x.Id))
                            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                            .OrderBy(x => searchResult.Items.IndexOf(x.Id))
                            .ToListAsync(cancellationToken);

            return new(
                items,
                searchResult.TotalCount,
                request.Pagination.Page,
                request.Pagination.Items
            );

            //return await _context.Posts
            //           .AsNoTracking()
            //           .Where(x =>
            //               x.IsPublished &&
            //               x.PublishDate <= DateTime.UtcNow &&
            //               postIds.Contains(x.Id))
            //           .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            //           .OrderBy(x => postIds.IndexOf(x.Id))
            //           .ToListAsync(cancellationToken);

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