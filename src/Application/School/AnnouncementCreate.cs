namespace SkorinosGimnazija.Application.School;
using AutoMapper;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using Dtos;
using Validators;

public static class AnnouncementCreate
{
    public record Command(AnnouncementCreateDto Announcement) : IRequest<AnnouncementDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Announcement).NotNull().SetValidator(new AnnouncementCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, AnnouncementDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<AnnouncementDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Announcements.Add(_mapper.Map<Announcement>(request.Announcement)).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<AnnouncementDto>(entity);
        }
    }
}
