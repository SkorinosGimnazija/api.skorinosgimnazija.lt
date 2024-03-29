﻿namespace SkorinosGimnazija.Application.Events;

using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

public static class EventPublicList
{
    public record Query(int? Week) : IRequest<List<EventDto>>;

    public class Handler : IRequestHandler<Query, List<EventDto>>
    {
        private readonly IMemoryCache _cache;
        private readonly ICalendarService _calendarService;

        public Handler(ICalendarService calendarService, IMemoryCache cache)
        {
            _calendarService = calendarService;
            _cache = cache;
        }

        public async Task<List<EventDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var key = $"EventsW{request.Week}";

            if (!_cache.TryGetValue(key, out List<EventDto>? cached))
            {
                DateTime start;
                DateTime end;

                if (request.Week is not null)
                {
                    start = DateTime.UtcNow.AddDays(request.Week.Value * 7).Date;
                    end = DateTime.UtcNow.AddDays((request.Week.Value + 1) * 7).Date;
                }
                else
                {
                    start = DateTime.UtcNow.Date;
                    end = DateTime.UtcNow.AddDays(1).Date;
                }

                cached = await _calendarService.GetEventsAsync(start, end, cancellationToken);
                _cache.Set(key, cached, TimeSpan.FromHours(1));
            }

            return cached!;
        }
    }
}