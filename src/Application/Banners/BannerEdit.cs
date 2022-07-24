namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities;
using Domain.Entities.CMS;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Infrastructure.Revalidation;
using Validators;

public static class BannerEdit
{
    public record Command(BannerEditDto Banner) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Banner).NotNull().SetValidator(new BannerEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRevalidationService _revalidation;
        private readonly IMediaManager _mediaManager;
        private readonly ISearchClient _searchClient;

        public Handler(IAppDbContext context,
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
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Banners.FirstOrDefaultAsync(x => x.Id == request.Banner.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.Banner, entity);

            await SaveSearchIndexAsync(entity);
            await SavePictureAsync(entity, request.Banner);

            await _context.SaveChangesAsync();

            await RevalidatePost(entity);

            return Unit.Value;
        }

        private async Task RevalidatePost(Banner entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug);
        }

        private async Task SavePictureAsync(Banner banner, BannerEditDto newBanner)
        {
            if (newBanner.Picture is null)
            {
                return;
            }

            _mediaManager.DeleteFile(banner.PictureUrl);

            var image = await _mediaManager.SaveFilesAsync(new[] { newBanner.Picture });
            banner.PictureUrl = image[0];
        }

        private async Task SaveSearchIndexAsync(Banner banner)
        {
            await _searchClient.SaveBannerAsync(_mapper.Map<BannerIndexDto>(banner));
        }
    }
}