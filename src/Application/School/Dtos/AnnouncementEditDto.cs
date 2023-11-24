namespace SkorinosGimnazija.Application.School.Dtos;

public record AnnouncementEditDto : AnnouncementCreateDto
{
    public int Id { get; init; }
}