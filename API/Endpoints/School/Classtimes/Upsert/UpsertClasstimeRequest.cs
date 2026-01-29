namespace API.Endpoints.School.Classtimes.Upsert;

using JetBrains.Annotations;

[PublicAPI]
public record UpsertClasstimeRequest
{
    public required int Id { get; init; }

    public required TimeOnly StartTime { get; init; }

    public TimeOnly? StartTimeShort { get; init; }

    public required TimeOnly EndTime { get; init; }

    public TimeOnly? EndTimeShort { get; init; }
}

public class UpsertClasstimeRequestValidator : Validator<UpsertClasstimeRequest>
{
    public UpsertClasstimeRequestValidator()
    {
        RuleFor(x => x.StartTimeShort)
            .Null().When(x => x.EndTimeShort == null)
            .WithMessage("Both shortened times must either be null or have values")
            .NotNull().When(x => x.EndTimeShort != null)
            .WithMessage("Both shortened time must either be null or have values");

        RuleFor(x => x.EndTimeShort)
            .Null().When(x => x.StartTimeShort == null)
            .WithMessage("Both shortened time must either be null or have values")
            .NotNull().When(x => x.StartTimeShort != null)
            .WithMessage("Both shortened time must either be null or have values");
    }
}