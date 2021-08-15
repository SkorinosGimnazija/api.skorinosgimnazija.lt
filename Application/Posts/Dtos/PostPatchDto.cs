﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Dtos
{

    public record PostPatchDto
    {
        public bool? IsFeatured { get; set; } = null;
        public bool? IsPublished { get; set; } = null;

    }
}
