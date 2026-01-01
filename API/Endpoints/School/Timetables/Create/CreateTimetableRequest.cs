namespace API.Endpoints.School.Timetables.Create;

using API.Database.Entities.School;
using JetBrains.Annotations;

[PublicAPI]
public record CreateTimetableRequest
{
    public required int DayId { get; init; }

    public required int TimeId { get; init; }

    public required int RoomId { get; init; }

    public required string ClassName { get; init; }
}

public class CreateTimetableRequestValidator : Validator<List<CreateTimetableRequest>>
{
    public CreateTimetableRequestValidator()
    {
        RuleFor(x => x).NotEmpty();
        RuleForEach(x => x)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ClassName)
                    .NotEmpty()
                    .MaximumLength(TimetableConfiguration.ClassNameLength);
            });
    }
}