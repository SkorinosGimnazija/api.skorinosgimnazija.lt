namespace SkorinosGimnazija.Application.Banners;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class BannerSearch
{
    public record Query(string SearchText, PaginationDto Pagination) : IRequest<PaginatedList<BannerDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
            RuleFor(x => x.SearchText).MaximumLength(50);
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<BannerDto>>
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

        public async Task<PaginatedList<BannerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchResult =
                await _search.SearchBannersAsync(request.SearchText, request.Pagination, cancellationToken);
            var items = await _context.Banners
                            .AsNoTracking()
                            .Where(x => searchResult.Items.Contains(x.Id))
                            .ProjectTo<BannerDto>(_mapper.ConfigurationProvider)
                            .OrderBy(x => searchResult.Items.IndexOf(x.Id))
                            .ToListAsync(cancellationToken);

            return new(
                items,
                searchResult.TotalCount,
                request.Pagination.Page,
                request.Pagination.Items
            );
        }
    }
}