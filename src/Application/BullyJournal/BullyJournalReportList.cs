namespace SkorinosGimnazija.Application.BullyJournal;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Extensions;
using SkorinosGimnazija.Application.Common.Pagination;

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

        public async Task<PaginatedList<BullyJournalReportDto>> Handle(
            Query request, CancellationToken cancellationToken)
        {
            return await _context.BullyJournalReports
                       .AsNoTracking()
                       .ProjectTo<BullyJournalReportDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}