namespace SkorinosGimnazija.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record  EmailOptions
{
    public string SenderName { get; init; } = default!;
    public string SenderEmail { get; init; } = default!;
}
