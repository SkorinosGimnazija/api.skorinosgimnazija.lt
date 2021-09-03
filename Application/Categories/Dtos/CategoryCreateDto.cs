using Domain.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Dtos
{
    public record CategoryCreateDto
    {
        public int LanguageId { get; init; }

        public string Name { get; init; }

        public string Slug { get; init; }


        public bool ShowOnHomePage { get; init; }
    }
}
