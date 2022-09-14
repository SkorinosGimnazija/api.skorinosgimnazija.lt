namespace SkorinosGimnazija.Application.Events;

using System.Diagnostics.CodeAnalysis;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class EventCreate
{
    public record Command(EventCreateDto Event) : IRequest<EventDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Event).NotNull().SetValidator(new CreateEventValidator());
        }
    }

    public class Handler : IRequestHandler<Command, EventDto>
    {
        private readonly ICalendarService _calendarService;

        public Handler(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<EventDto> Handle(Command request, CancellationToken _)
        {
            var eventId = await _calendarService.AddEventAsync(
                              request.Event.Title,
                              request.Event.StartDate,
                              request.Event.EndDate,
                              request.Event.AllDay);

            return new() { Id = eventId };
        }
    }
}