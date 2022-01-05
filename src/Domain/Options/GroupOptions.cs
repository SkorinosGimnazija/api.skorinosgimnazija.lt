namespace SkorinosGimnazija.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record GroupOptions
{ 
    public string Service { get; init; } = default!;
    public string Managers { get; init; } = default!;
    public string Teachers { get; init; } = default!;
    public string BullyManagers { get; init; } = default!;

}
