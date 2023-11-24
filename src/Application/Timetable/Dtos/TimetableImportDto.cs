namespace SkorinosGimnazija.Application.Timetable.Dtos;

public record TimetableImportDto
{
    public List<TimetableCreateDto> TimetableList { get; init; } = new();
}