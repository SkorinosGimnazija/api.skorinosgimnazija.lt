namespace SkorinosGimnazija.Application.Banners;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BannerDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly ISearchClient _searchClient;
        private readonly IMediaManager _mediaManager;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMediaManager mediaManager)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Banners.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            await _searchClient.RemoveBannerAsync(entity);

            _mediaManager.DeleteFile(entity.PictureUrl);

            _context.Banners.Remove(entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
