namespace SkorinosGimnazija.Domain.Entities.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Classtime
{
    public int Id { get; set; }

    public int Number { get; set; }

    public TimeOnly StartTime { get; init; }

    public TimeOnly EndTime { get; init; }
}
