using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FileManager
{
   public record FileManagerSettings
    {
        public string UploadPath { get; set; }
    }
}
