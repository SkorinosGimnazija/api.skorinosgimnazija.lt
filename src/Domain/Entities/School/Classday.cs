namespace SkorinosGimnazija.Domain.Entities.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Classday
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int Number { get; set; }
}
