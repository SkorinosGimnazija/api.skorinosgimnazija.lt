namespace Application.Categories.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record CategoryDto
    {
        public int Id { get; init; }

        public int LanguageId { get; init; }

        public string Name { get; init; }

        public string Slug { get; init; }


        public bool ShowOnHomePage { get; init; }
    }
}
