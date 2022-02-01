namespace SkorinosGimnazija.Application.Courses.Dtos;

public record CourseEditDto : CourseCreateDto
{
    public int Id { get; init; }
}