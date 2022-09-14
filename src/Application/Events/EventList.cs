namespace SkorinosGimnazija.Application.Events;

using Common.Interfaces;
using Dtos;
using MediatR;

public static class EventList
{
    public record Query(DateTime Start, DateTime End) : IRequest<List<EventDto>>;

    public class Handler : IRequestHandler<Query, List<EventDto>>
    {
        private readonly ICalendarService _calendarService;

        public Handler(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public async Task<List<EventDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var start = request.Start.Date;
            var end = request.End.Date.AddDays(1).AddSeconds(-1);

            return await _calendarService.GetEventsAsync(start, end, cancellationToken);
        }
    }
}