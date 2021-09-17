namespace Application.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public record PaginationDto
    {
        private int _items = 20;
        private int _page = 0;

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
