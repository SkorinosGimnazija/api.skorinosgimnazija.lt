namespace SkorinosGimnazija.Application.Accomplishments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AccomplishmentCreateStudentDto
{
    public int ClassroomId { get; init; }

    public string Name { get; init; } = default!;
}
