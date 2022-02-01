namespace SkorinosGimnazija.Application.BullyReports;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class BullyReportDetails
{
    public record Query(int Id) : IRequest<BullyReportDto>;

    public class Handler : IRequestHandler<Query, BullyReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BullyReportDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.BullyReports
                             .AsNoTracking()
                             .ProjectTo<BullyReportDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return entity;
        }
    }
}