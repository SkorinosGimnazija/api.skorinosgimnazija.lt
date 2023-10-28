namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Pagination;

public static class ClasstimeShortDaysList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<ClasstimeShortDayDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<ClasstimeShortDayDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ClasstimeShortDayDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.ClasstimeShortDays
                       .AsNoTracking()
                       .ProjectTo<ClasstimeShortDayDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}