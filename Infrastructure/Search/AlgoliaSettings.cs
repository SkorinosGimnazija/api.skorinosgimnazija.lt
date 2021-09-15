namespace Infrastructure.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public record AlgoliaSettings 
    {
        public  string  AppId { get; set; }  
        public string ApiKey { get; set; }
    }
}
