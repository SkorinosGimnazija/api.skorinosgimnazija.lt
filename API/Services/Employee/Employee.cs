namespace API.Services.Employee;

public record Employee
{
    public required string EmployeeId { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }

    public required string UnitPath { get; init; }

    public required bool IsSuspended { get; init; }

    public string? JobTitle { get; init; }

    public string? Location { get; init; }

    public bool IsTeacher
    {
        get
        {
            const string TeacherPath = "/Teachers";
            return UnitPath.StartsWith(TeacherPath);
        }
    }

    public bool IsHeadTeacher
    {
        get
        {
            const string HeadTeacherPath = "/Teachers/HeadTeachers";
            return UnitPath.StartsWith(HeadTeacherPath);
        }
    }

    public bool IsPrincipal
    {
        get
        {
            const string PrincipalPath = "/Teachers/Principal";
            return UnitPath.StartsWith(PrincipalPath);
        }
    }
}