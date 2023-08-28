namespace SkorinosGimnazija.Domain.Entities.Timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School;

public class Timetable
{
    public int Id { get; set; }

    public Classday Day { get; set; } = default!;

    public int DayId { get; set; }

    public Classtime Time { get; set; } = default!;
    
    public int TimeId { get; set; }

    public Classroom Room { get; set; } = default!;
    
    public int RoomId { get; set; }

    public string? ClassName { get; set; }
}
