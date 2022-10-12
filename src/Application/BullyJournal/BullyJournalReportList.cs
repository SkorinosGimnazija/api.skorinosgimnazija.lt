namespace SkorinosGimnazija.Application.BullyJournalReports;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using BullyReports.Dtos;
using Common.Extensions;
using Common.Interfaces;
using Common.Pagination;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class BullyJournalReportList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<BullyJournalReportDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<BullyJournalReportDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BullyJournalReportDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.BullyJournalReports
                       .AsNoTracking()
                       .ProjectTo<BullyJournalReportDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}