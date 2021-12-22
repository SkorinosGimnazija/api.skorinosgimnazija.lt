namespace SkorinosGimnazija.Application.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record CourseEditDto : CourseCreateDto
{
    public int Id { get; init; }
}
 