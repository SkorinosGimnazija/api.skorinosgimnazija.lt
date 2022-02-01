namespace SkorinosGimnazija.Application.Languages;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PublicLanguageList
{
    public record Query() : IRequest<List<LanguageDto>>;

    public class Handler : IRequestHandler<Query, List<LanguageDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LanguageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Languages
                       .AsNoTracking()
                       .ProjectTo<LanguageDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}