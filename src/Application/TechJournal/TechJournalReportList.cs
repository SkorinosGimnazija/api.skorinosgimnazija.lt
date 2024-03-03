namespace SkorinosGimnazija.Application.TechJournal;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class TechJournalReportList
{
    public record Query(
        PaginationDto Pagination,
        DateOnly StartDate,
        DateOnly EndDate) : IRequest<PaginatedList<TechJournalReportDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<TechJournalReportDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TechJournalReportDto>> Handle(
            Query request, CancellationToken cancellationToken)
        {
            var startDate = request.StartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var endDate = request.EndDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

            return await _context.TechJournalReports
                       .AsNoTracking()
                       .ProjectTo<TechJournalReportDto>(_mapper.ConfigurationProvider)
                       .Where(x => x.ReportDate >= startDate && x.ReportDate <= endDate)
                       .OrderByDescending(x => x.IsFixed == null)
                       .ThenByDescending(x => x.IsFixed == false)
                       .ThenByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}