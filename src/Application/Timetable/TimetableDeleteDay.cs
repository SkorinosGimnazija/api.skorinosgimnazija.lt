namespace SkorinosGimnazija.Application.Timetable;

using System.Diagnostics.CodeAnalysis;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class TimetableDeleteDay
{
    public record Command(TimetableDeleteDayDto Days) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;

        public Handler(IAppDbContext context)
        {
            _context = context;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            await _context.Timetable
                .Where(x => request.Days.DayIds.Contains(x.DayId))
                .ExecuteDeleteAsync();

            return Unit.Value;
        }
    }
}