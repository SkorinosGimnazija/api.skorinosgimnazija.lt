namespace SkorinosGimnazija.Infrastructure.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record SearchObject
{
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;
}
