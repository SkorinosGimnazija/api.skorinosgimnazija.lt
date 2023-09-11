namespace SkorinosGimnazija.Application.Timetable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TimetableCreateDto
{
    public int DayId { get; set; }

    public int TimeId { get; set; }

    public int RoomId { get; set; }

    public string ClassName { get; set; } = default!;
}
