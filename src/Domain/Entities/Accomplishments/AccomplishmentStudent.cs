namespace SkorinosGimnazija.Domain.Entities.Accomplishments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AccomplishmentStudent
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int AccomplishmentId { get; set; }

    public Accomplishment Accomplishment { get; set; } = default!;

    public int ClassroomId { get; set; }

    public AccomplishmentClassroom Classroom { get; set; } = default!;
}
