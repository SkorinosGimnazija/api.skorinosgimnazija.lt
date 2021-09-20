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