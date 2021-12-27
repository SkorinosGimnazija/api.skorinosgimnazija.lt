namespace SkorinosGimnazija.Application.BullyReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record BullyReportEditDto : BullyReportCreateDto
{
    public int Id { get; init; }
}
