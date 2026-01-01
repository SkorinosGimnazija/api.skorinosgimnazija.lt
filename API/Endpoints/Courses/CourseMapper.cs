namespace API.Endpoints.Courses;

using API.Database.Entities.Courses;
using API.Endpoints.Courses.Create;

public sealed class CourseMapper
    : Mapper<CreateCourseRequest, CourseResponse, Course>
{
    public override Course ToEntity(CreateCourseRequest r)
    {
        return new()
        {
            Title = r.Title.Trim(),
            Organizer = r.Organizer.Trim(),
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            DurationInHours = r.DurationInHours,
            Certificate = r.Certificate?.Trim(),
            IsUseful = r.IsUseful,
            CreatorId = r.CreatorId
        };
    }

    public override CourseResponse FromEntity(Course e)
    {
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Organizer = e.Organizer,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            DurationInHours = e.DurationInHours,
            Certificate = e.Certificate,
            IsUseful = e.IsUseful,
            CreatorId = e.Creator.Id,
            CreatorName = e.Creator.Name
        };
    }

    public override Course UpdateEntity(CreateCourseRequest r, Course e)
    {
        e.Title = r.Title.Trim();
        e.Organizer = r.Organizer.Trim();
        e.StartDate = r.StartDate;
        e.EndDate = r.EndDate;
        e.DurationInHours = r.DurationInHours;
        e.Certificate = r.Certificate?.Trim();
        e.IsUseful = r.IsUseful;

        return e;
    }
}