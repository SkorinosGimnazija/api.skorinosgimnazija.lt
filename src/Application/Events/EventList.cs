namespace SkorinosGimnazija.Application.Events;

using Common.Interfaces;
using Dtos;
using MediatR;

public static class EventList
{
    public record Query(int? Week) : IRequest<List<EventDto>>;

    public class Handler : IRequestHandler<Query, List<EventDto>>
    {
        private readonly ICalendarService _calendarService;

        public Handler(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public async Task<List<EventDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            DateTime start;
            DateTime end;

            if (request.Week is not null)
            {
                start = DateTime.Now.AddDays(request.Week.Value * 7).Date;
                end = DateTime.Now.AddDays((request.Week.Value + 1) * 7).Date;
            }
            else
            {
                start = DateTime.Now.Date;
                end = DateTime.Now.AddDays(1).Date;
            }

            return await _calendarService.GetEventsAsync(start, end, cancellationToken);
        }
    }
}