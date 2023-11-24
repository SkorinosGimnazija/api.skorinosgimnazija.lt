namespace SkorinosGimnazija.Application.TechJournal;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class TechJournalReportDetails
{
    public record Query(int Id) : IRequest<TechJournalReportDto>;

    public class Handler : IRequestHandler<Query, TechJournalReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TechJournalReportDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.TechJournalReports
                             .AsNoTracking()
                             .ProjectTo<TechJournalReportDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}