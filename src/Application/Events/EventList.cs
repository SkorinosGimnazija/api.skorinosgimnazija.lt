namespace SkorinosGimnazija.Application.Events;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Extensions;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Posts;

public static class EventList
{
    public record Query(int? Week) : IRequest<List<EventDto>>;
     
    public class Handler : IRequestHandler<Query, List<EventDto>>
    {
        private readonly ICalendarClient _calendarClient;

        public Handler(ICalendarClient calendarClient)
        {
            _calendarClient = calendarClient;
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

            return await _calendarClient.GetEventsAsync(start, end, cancellationToken);
        }
    }
}