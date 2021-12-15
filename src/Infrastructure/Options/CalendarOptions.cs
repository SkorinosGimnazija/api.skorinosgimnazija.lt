namespace SkorinosGimnazija.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public  record CalendarOptions
{
    public string ParentAppointmentsCalendarId { get; init; } = default!;
    public string EventsCalendarId { get; init; } = default!;
    public string User { get; init; } = default!;
}
  