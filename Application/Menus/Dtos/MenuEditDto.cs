using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Menus.Dtos
{
    public record MenuEditDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public bool IsPublished { get; set; }
        public int DomainId { get; set; }
        public int CategoryId { get; set; }
        public string? Url { get; set; }
        public int? ParentMenuId { get; set; }
    }
}
