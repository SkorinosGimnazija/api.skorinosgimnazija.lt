namespace API.Endpoints.Events.Create;

using JetBrains.Annotations;

[PublicAPI]
public record CreateCalendarEventRequest
{
    public required string Title { get; init; }

    public required DateTime StartDate { get; init; }

    public required DateTime EndDate { get; init; }

    public required bool AllDay { get; init; }
}

public class CreateCalendarEventRequestValidator : Validator<CreateCalendarEventRequest>
{
    public CreateCalendarEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}