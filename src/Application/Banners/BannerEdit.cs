namespace SkorinosGimnazija.Application.Banners;
using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using MediatR;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Banners.Dtos;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menus.Validators;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMediaManager _mediaManager;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMediaManager mediaManager, IMapper mapper)
        {
            _context = context;
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

            if (request.Banner.Picture is not null)
            {
                _mediaManager.DeleteFile(entity.PictureUrl);
                var image = await _mediaManager.SaveFilesAsync(new[] { request.Banner.Picture });
                entity.PictureUrl = image[0];
            }

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
