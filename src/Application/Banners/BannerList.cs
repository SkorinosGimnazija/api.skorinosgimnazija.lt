namespace SkorinosGimnazija.Application.Banners;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Extensions;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Posts;

public static class BannerList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<BannerDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<BannerDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BannerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Banners
                       .AsNoTracking()
                       .ProjectTo<BannerDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => x.Order)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}