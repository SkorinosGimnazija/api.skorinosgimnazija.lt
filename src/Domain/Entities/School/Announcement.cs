namespace SkorinosGimnazija.Domain.Entities.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public DateOnly StartTime { get; set; }

    public DateOnly EndTime { get; set; }
}
