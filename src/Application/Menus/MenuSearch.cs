namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class MenuSearch
{
    public record Query(string SearchText, PaginationDto Pagination) : IRequest<PaginatedList<MenuDetailsDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
            RuleFor(x => x.SearchText).MaximumLength(50);
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<MenuDetailsDto>>
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

        public async Task<PaginatedList<MenuDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchResult = await _search.SearchMenuAsync(request.SearchText, request.Pagination, cancellationToken);
            var items = await _context.Menus
                            .AsNoTracking()
                            .Where(x => searchResult.Items.Contains(x.Id))
                            .ProjectTo<MenuDetailsDto>(_mapper.ConfigurationProvider)
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