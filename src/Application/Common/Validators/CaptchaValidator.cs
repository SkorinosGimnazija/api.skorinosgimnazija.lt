﻿namespace SkorinosGimnazija.Application.Common.Validators;

using FluentValidation;
using Interfaces;

public class CaptchaValidator : AbstractValidator<string>
{
    private readonly ICaptchaService _captchaService;

    public CaptchaValidator(ICaptchaService captchaService)
    {
        _captchaService = captchaService;

        RuleFor(x => x).MustAsync(ValidCaptcha).WithMessage("Invalid captcha");
    }

    private async Task<bool> ValidCaptcha(string token, CancellationToken ct)
    {
        return await _captchaService.ValidateAsync(token);
    }
}