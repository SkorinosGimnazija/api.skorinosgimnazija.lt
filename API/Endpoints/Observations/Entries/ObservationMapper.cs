namespace API.Endpoints.Observations.Entries;

using API.Database.Entities.Observations;
using API.Endpoints.Observations.Entries.Create;

public sealed class ObservationMapper
    : Mapper<CreateObservationRequest, ObservationResponse, Observation>
{
    public override Observation ToEntity(CreateObservationRequest r)
    {
        return new()
        {
            Date = r.Date,
            Note = string.IsNullOrWhiteSpace(r.Note) ? null : r.Note.Trim(),
            StudentId = r.StudentId,
            LessonId = r.LessonId,
            CreatorId = r.CreatorId
        };
    }

    public override ObservationResponse FromEntity(Observation e)
    {
        return new()
        {
            Id = e.Id,
            Date = e.Date,
            Note = e.Note,
            StudentId = e.StudentId,
            StudentName = e.Student.Name,
            LessonId = e.LessonId,
            LessonName = e.Lesson.Name,
            CreatorName = e.Creator.Name,
            OptionIds = e.Options.Select(x => x.Id).ToList()
        };
    }

    public override Observation UpdateEntity(CreateObservationRequest r, Observation e)
    {
        e.Date = r.Date;
        e.Note = string.IsNullOrWhiteSpace(r.Note) ? null : r.Note.Trim();
        e.StudentId = r.StudentId;
        e.LessonId = r.LessonId;

        return e;
    }
}