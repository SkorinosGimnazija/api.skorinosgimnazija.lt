namespace SkorinosGimnazija.Application.School;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.School;
using Dtos;
using FluentValidation;
using MediatR;
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