namespace Application.Extensions;

using Domain.CMS;

internal static class PostExtensions
{
    public static void GenerateFileLinks(this Post post, string staticUrl)
    {
        const string Search = "{auto-file-link}";
        var replace = $"{staticUrl}/uploads/{post.Id}";

        if (post.IntroText is not null)
        {
            post.IntroText = post.IntroText.Replace(Search, replace);
        }

        if (post.Text is not null)
        {
            post.Text = post.Text.Replace(Search, replace);
        }
    }
}