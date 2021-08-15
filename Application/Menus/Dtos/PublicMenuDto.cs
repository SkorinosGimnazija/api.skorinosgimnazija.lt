namespace Application.Menus.Dtos
{
    using System;
    using System.Collections.Generic;
    using Domain.CMS;

    public record PublicMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Slug { get; set; }

        public string? Url { get; set; }


        public int? ParentMenuId { get; set;}
    }
}