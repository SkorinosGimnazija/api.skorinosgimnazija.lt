namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TimetableImportDto
{
    public List<TimetableCreateDto> TimetableList { get; init; } = new();
}
