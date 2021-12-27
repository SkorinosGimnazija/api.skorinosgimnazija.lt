namespace SkorinosGimnazija.Application.BullyReports;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Posts;

public static class BullyReportList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<BullyReportDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<BullyReportDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BullyReportDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.BullyReports
                       .AsNoTracking()
                       .ProjectTo<BullyReportDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}