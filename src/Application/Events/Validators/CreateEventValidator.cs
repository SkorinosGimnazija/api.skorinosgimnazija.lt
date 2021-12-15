namespace SkorinosGimnazija.Application.Events.Validators;
using FluentValidation;
using SkorinosGimnazija.Application.Banners.Dtos;

using SkorinosGimnazija.Application.Banners.Validators;
using SkorinosGimnazija.Application.Events.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class CreateEventValidator : AbstractValidator<EventCreateDto>
{
    public CreateEventValidator()
    {
     
    }
}
