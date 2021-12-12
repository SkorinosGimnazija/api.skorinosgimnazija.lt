namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities;
using Dtos;
using FluentValidation;
using MediatR;
using Menus.Validators;
using SkorinosGimnazija.Application.Menus.Dtos;
using Validators;

public static class BannerCreate
{
    public record Command(BannerCreateDto Banner) : IRequest<BannerDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Banner).NotNull().SetValidator(new BannerCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, BannerDto>
    {
        private readonly IAppDbContext _context;
        private readonly ISearchClient _searchClient;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMediaManager mediaManager, IMapper mapper)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<BannerDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Banners.Add(_mapper.Map<Banner>(request.Banner)).Entity;

            await SaveSearchIndexAsync(entity);
            await SavePictureAsync(entity, request.Banner);

            await _context.SaveChangesAsync();

            return _mapper.Map<BannerDto>(entity);
        }

        private async Task SavePictureAsync(Banner banner, BannerCreateDto newBanner)
        {
            var image = await _mediaManager.SaveFilesAsync(new[] { newBanner.Picture });
            banner.PictureUrl = image[0];
        }


        private async Task SaveSearchIndexAsync(Banner banner)
        {
            await _searchClient.SaveBannerAsync(_mapper.Map<BannerIndexDto>(banner));
        }
    }
}