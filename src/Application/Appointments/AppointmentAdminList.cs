namespace SkorinosGimnazija.Application.Appointments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AppointmentAdminList
{
    public record Query(PaginationDto Pagination, string? SearchQuery) : IRequest<PaginatedList<AppointmentDetailsDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<AppointmentDetailsDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AppointmentDetailsDto>> Handle(
            Query request, CancellationToken cancellationToken)
        {
            var list = _context.Appointments
                .AsNoTracking()
                .ProjectTo<AppointmentDetailsDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                list = list.Where(x => x.UserDisplayName.ToLower().Contains(request.SearchQuery.ToLower()));
                //list = list.Where(x => EF.Functions.Like(x.UserDisplayName, $"%{request.SearchQuery}%"));
            }

            return await list.OrderByDescending(x => x.Date.Date)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}