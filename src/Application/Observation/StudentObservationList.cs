namespace SkorinosGimnazija.Application.Observation;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Common.Pagination;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class StudentObservationList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<StudentObservationDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler(IAppDbContext context, IMapper mapper)
        : IRequestHandler<Query, PaginatedList<StudentObservationDto>>
    {
        public async Task<PaginatedList<StudentObservationDto>> Handle(Query request, CancellationToken ct)
        {
            return await context.StudentObservations
                       .AsNoTracking()
                       .ProjectTo<StudentObservationDto>(mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, ct);
        }
    }
}