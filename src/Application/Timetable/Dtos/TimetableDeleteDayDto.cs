namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TimetableDeleteDayDto
{
    public List<int> DayIds { get; set; } = new();
}
