namespace SkorinosGimnazija.Application.Accomplishments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AccomplishmentAchievementDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;
}
