using Domain.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Dtos
{
    public record CategoryEditDto
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }


        public bool ShowOnHomePage { get; set; }
    }
}