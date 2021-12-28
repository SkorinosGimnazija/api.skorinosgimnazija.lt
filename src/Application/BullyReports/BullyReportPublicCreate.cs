namespace SkorinosGimnazija.Application.BullyReports;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Bullies;
using Domain.Entities.Teacher;
using Dtos;
using FluentValidation;
using Infrastructure.Captcha;
using Infrastructure.Email;
using MediatR;
using Menus.Validators;
using SkorinosGimnazija.Application.BullyReports.Events;
using SkorinosGimnazija.Application.Courses.Validators;
using SkorinosGimnazija.Application.Menus.Dtos;
 
public static class BullyReportPublicCreate
{
    public record Command(BullyReportCreateDto BullyReport) : IRequest<BullyReportDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator(ICaptchaService captchaService)
        {
            RuleFor(v => v.BullyReport).NotNull().SetValidator(new BullyReportCreateValidator(captchaService));
        }
    }

    public class Handler : IRequestHandler<Command, BullyReportDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;

        public Handler(IAppDbContext context, IMapper mapper, IPublisher publisher)
        {
            _context = context;
            _mapper = mapper;
            _publisher = publisher;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<BullyReportDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.BullyReports.Add(_mapper.Map<BullyReport>(request.BullyReport)).Entity;
             
            await _context.SaveChangesAsync();

            await _publisher.Publish(new BullyReportCreatedNotification(entity));
             
            return _mapper.Map<BullyReportDto>(entity);
        }


    }
}