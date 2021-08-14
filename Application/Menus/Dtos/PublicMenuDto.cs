namespace Application.Menus.Dtos
{
    using System;
    using System.Collections.Generic;
    using Domain.CMS;

    public record PublicMenuDto
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public string? Slug { get; init; }

        public string? Url { get; init; }


        public int? ParentMenuId { get; init;}
    }
}