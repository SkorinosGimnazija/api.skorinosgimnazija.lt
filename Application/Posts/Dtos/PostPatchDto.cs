using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Dtos
{
    public record PostPatchDto
    {
        public bool? IsFeatured { get; init; }
        public bool? IsPublished { get; init; }
    }
}
