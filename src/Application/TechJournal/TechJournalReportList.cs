namespace SkorinosGimnazija.Application.TechJournal;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Extensions;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Common.Pagination;

public static class TechJournalReportList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<TechJournalReportDto>>;

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

        public async Task<PaginatedList<TechJournalReportDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.TechJournalReports
                       .AsNoTracking()
                       .ProjectTo<TechJournalReportDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.IsFixed == null)
                       .ThenByDescending(x => x.IsFixed == false)
                       .ThenByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}