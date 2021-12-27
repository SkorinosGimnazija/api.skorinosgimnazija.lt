namespace SkorinosGimnazija.Application.BullyReports;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Domain.Entities;
using Validators;

public static class BullyReportEdit
{
    public record Command(BullyReportEditDto BullyReport) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.BullyReport).NotNull().SetValidator(new BullyReportEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.BullyReports.FirstOrDefaultAsync(x => x.Id == request.BullyReport.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.BullyReport, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}