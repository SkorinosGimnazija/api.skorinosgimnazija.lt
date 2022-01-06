namespace SkorinosGimnazija.Application.ParentAppointments;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Appointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Common.Pagination;
using FluentValidation;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Application.Common.Extensions;

public  static class AppointmentAdminList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<AppointmentDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<AppointmentDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                       .AsNoTracking()
                       .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Date.Date)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}
