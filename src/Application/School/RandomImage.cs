namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using MediatR;

using SkorinosGimnazija.Application.School.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class RandomImage
{
    public record Query() : IRequest<string?>;

    public class Handler : IRequestHandler<Query, string?>
    {
        private static readonly Random Random = new();
        private readonly IAppDbContext _context;
        private readonly IMemoryCache _cache;

        public Handler(IAppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
        {
            const string CacheKey = "RandomImage";

            if (!_cache.TryGetValue<string>(CacheKey, out var image))
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
                image = images[Random.Next(images.Count)];
                _cache.Set(CacheKey, image, TimeSpan.FromMinutes(5));
            }

            return image;
        }
    }
}
