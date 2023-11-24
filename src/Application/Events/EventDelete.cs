namespace SkorinosGimnazija.Application.Events;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using MediatR;

public static class EventDelete
{
    public record Command(string Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ICalendarService _calendarService;

        public Handler(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var result = await _calendarService.DeleteEventAsync(request.Id);

            if (!result)
            {
                throw new NotFoundException();
            }

            return Unit.Value;
        }
    }
}