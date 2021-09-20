namespace Application.Dtos
{
    using System;

    public record PaginationDto
    {
        private readonly int _items = 20;
        private readonly int _page = 0;

        public int Items
        {
            get { return _items; }
            init { _items = Math.Clamp(value, 1, _items); }
        }

        public int Page
        {
            get { return _page; }
            init { _page = Math.Max(value - 1, 0); }
        }
    }
}