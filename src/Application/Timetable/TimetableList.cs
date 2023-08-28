namespace SkorinosGimnazija.Application.Timetable;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Dtos;
using Microsoft.EntityFrameworkCore;

public static class TimetableList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<TimetableDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<TimetableDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TimetableDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Timetable
                       .AsNoTracking()
                       .ProjectTo<TimetableDto>(_mapper.ConfigurationProvider)
                       .OrderBy(x => x.Room.Number)
                       .ThenBy(x => x.Day.Number)
                       .ThenBy(x => x.Time.Number)
                       .ThenBy(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}
