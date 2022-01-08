namespace SkorinosGimnazija.Application.ParentAppointments;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;

using SkorinosGimnazija.Application.ParentAppointments.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public  static class AppointmentDatesList
{
    public record Query(string TypeSlug) : IRequest<List<AppointmentDateDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TypeSlug).NotEmpty().MaximumLength(100);
        }
    }
     
    public class Handler : IRequestHandler<Query, List<AppointmentDateDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDates.AsNoTracking()
                       .Where(x => x.Type.Slug == request.TypeSlug)
                       .OrderBy(x=> x.Date)
                       .ProjectTo<AppointmentDateDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }


}
