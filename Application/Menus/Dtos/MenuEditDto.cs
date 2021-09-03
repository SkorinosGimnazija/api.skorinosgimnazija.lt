using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Menus.Dtos
{
    public record MenuEditDto
    {
        public int Id { get; init; }
        public int Order { get; init; }
        public string Name { get; init; }
        public string? Slug { get; init; }
        public bool IsPublished { get; init; }
        public int CategoryId { get; init; }
        public string? Url { get; init; }
        public int? ParentMenuId { get; init; }
    }
}
