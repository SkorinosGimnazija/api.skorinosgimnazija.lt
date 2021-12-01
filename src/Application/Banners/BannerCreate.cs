namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities;
using Dtos;
using FluentValidation;
using MediatR;
using Menus.Validators;
using Validators;

public static class BannerCreate
{
    public record Command(BannerCreateDto Banner) : IRequest<BannerDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Banner).SetValidator(new BannerCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, BannerDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMediaManager mediaManager, IMapper mapper)
        {
            _context = context;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<BannerDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Banners.Add(_mapper.Map<Banner>(request.Banner)).Entity;

            var image = await _mediaManager.SaveFilesAsync(new[] { request.Banner.Picture });

            entity.PictureUrl = image[0];

            await _context.SaveChangesAsync();

            return _mapper.Map<BannerDto>(entity);
        }
    }
}