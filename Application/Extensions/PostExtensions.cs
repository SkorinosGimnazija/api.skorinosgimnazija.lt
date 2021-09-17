using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    internal static class PostExtensions
    {
        public static string GenerateFileLinks(this string content, int postId, string staticUrl)
        {
            const string Search = "{auto-file-link}";
            var replace = $"{staticUrl}/{postId}";

            return content.Replace(Search, replace);
        }
    }
}
