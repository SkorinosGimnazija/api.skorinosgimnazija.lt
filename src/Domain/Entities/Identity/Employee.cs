namespace SkorinosGimnazija.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Employee
{
    public string Id { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
}
