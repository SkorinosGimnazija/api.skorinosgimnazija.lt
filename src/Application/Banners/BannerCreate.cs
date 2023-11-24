namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.CMS;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly IRevalidationService _revalidation;
        private readonly ISearchClient _searchClient;

        public Handler(
            IAppDbContext context,
            ISearchClient searchClient,
            IMediaManager mediaManager,
            IMapper mapper,
            IRevalidationService revalidation)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _revalidation = revalidation;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<BannerDto> Handle(Command request, CancellationToken _)
        {
            await using var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Banners.Add(_mapper.Map<Banner>(request.Banner)).Entity;

            await SavePictureAsync(entity, request.Banner);

            await _context.SaveChangesAsync();

            await SaveSearchIndexAsync(entity);

            await transaction.CommitAsync();

            await RevalidatePost(entity);

            return _mapper.Map<BannerDto>(entity);
        }

        private async Task RevalidatePost(Banner entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug);
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