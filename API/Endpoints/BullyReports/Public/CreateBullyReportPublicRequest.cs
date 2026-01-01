namespace API.Endpoints.BullyReports.Public;

using API.Database.Entities.BullyReports;
using API.Services.Captcha;
using JetBrains.Annotations;

[PublicAPI]
public record CreateBullyReportPublicRequest
{
    public required DateOnly Date { get; init; }

    public required string VictimName { get; init; }

    public required string BullyName { get; init; }

    public required string Location { get; init; }

    public required string Details { get; init; }

    public string? Observers { get; init; }

    public required string CaptchaToken { get; init; }
}

public class CreateBullyReportPublicRequestValidator : Validator<CreateBullyReportPublicRequest>
{
    public CreateBullyReportPublicRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotNull();

        RuleFor(x => x.VictimName)
            .NotNull()
            .MaximumLength(BullyReportConfiguration.NameLength);

        RuleFor(x => x.BullyName)
            .NotNull()
            .MaximumLength(BullyReportConfiguration.NameLength);

        RuleFor(x => x.Location)
            .NotNull()
            .MaximumLength(BullyReportConfiguration.LocationLength);

        RuleFor(x => x.Details)
            .NotNull()
            .MaximumLength(BullyReportConfiguration.DetailsLength);

        RuleFor(x => x.Observers)
            .MaximumLength(BullyReportConfiguration.DetailsLength);

        RuleFor(x => x.CaptchaToken)
            .MustAsync(ValidCaptcha)
            .WithMessage("Captcha klaida");
    }

    private async Task<bool> ValidCaptcha(string? captcha, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(captcha))
        {
            return false;
        }

        var captchaService = Resolve<CaptchaService>();
        return await captchaService.ValidateAsync(captcha);
    }
}