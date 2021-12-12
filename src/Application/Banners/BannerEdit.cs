﻿namespace SkorinosGimnazija.Application.Banners;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Domain.Entities;
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
        private readonly IMediaManager _mediaManager;
        private readonly ISearchClient _searchClient;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMediaManager mediaManager, IMapper mapper)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
            _mapper = mapper;
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

            return Unit.Value;
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