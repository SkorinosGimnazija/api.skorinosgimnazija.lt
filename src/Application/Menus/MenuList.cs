namespace SkorinosGimnazija.Application.Menus;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Posts;
using SkorinosGimnazija.Application.Common.Pagination;

public static class MenuList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<MenuDetailsDto>>;

    public class Validator : AbstractValidator<PostList.Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<MenuDetailsDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<MenuDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Menus
                       .AsNoTracking()
                       .ProjectTo<MenuDetailsDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => x.Order)
                       .ThenBy(x=> x.Path)
                       .PaginateToListAsync(request.Pagination, cancellationToken);
        }
    }
}