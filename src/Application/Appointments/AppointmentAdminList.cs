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
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<AppointmentDetailsDto>>;

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
            return await _context.Appointments
                       .AsNoTracking()
                       .ProjectTo<AppointmentDetailsDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Date.Date)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}