namespace SkorinosGimnazija.Application.School;

using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class RandomImage
{
    public record Query() : IRequest<RandomImageDto?>;

    public class Handler : IRequestHandler<Query, RandomImageDto?>
    {
        private static readonly Random Random = new();
        private readonly IMemoryCache _cache;
        private readonly IAppDbContext _context;

        public Handler(IAppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<RandomImageDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            const string CacheKey = "RandomImage";

            if (!_cache.TryGetValue<RandomImageDto>(CacheKey, out var image))
            {
                var randomPost = await _context.Posts
                                     .AsNoTracking()
                                     .Where(x => x.IsPublished && x.ShowInFeed &&
                                                 x.Images != null && x.Images.Count > 0)
                                     .OrderBy(x => EF.Functions.Random())
                                     .FirstOrDefaultAsync(cancellationToken);

                if (randomPost is null)
                {
                    return null;
                }

                var images = randomPost.Images!;
                image = new()
                {
                    Url = images[Random.Next(images.Count)]
                };

                _cache.Set(CacheKey, image, TimeSpan.FromMinutes(1));
            }

            return image;
        }
    }
}