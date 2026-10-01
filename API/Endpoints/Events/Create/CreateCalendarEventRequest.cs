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

public class CreateCalendarEventRequestValidator : Validator<List<CreateCalendarEventRequest>>
{
    public CreateCalendarEventRequestValidator()
    {
        RuleFor(x => x).NotEmpty();
        RuleForEach(x => x)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.Title)
                    .NotEmpty()
                    .MaximumLength(256);

                item.RuleFor(x => x.StartDate)
                    .NotEmpty();

                item.RuleFor(x => x.EndDate)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(x => x.StartDate);
            });
    }
}