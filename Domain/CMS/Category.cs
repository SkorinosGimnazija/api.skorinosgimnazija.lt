using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CMS
{
  public  class Category
    {
        public int Id { get; init; }
        
        public string Name { get; set; }

        public string Slug { get; set; }
        
        public Language Language{ get; set; }

        public bool ShowOnHomePage { get; set; }
    }
}
